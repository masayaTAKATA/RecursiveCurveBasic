using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace RecursiveCurveBasic
{
    public class KochCurveComponent : GH_Component
    {
        //Constructor
        public KochCurveComponent() : base("Koch curve", "KochCRV", "Generate the Koch curve form two point", "Meenaxy", "Default")
        {
        }

        #region Input, Output
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Number of depth", "num", "Number of recursion depth", GH_ParamAccess.item);
            pManager.AddPointParameter("Start point", "startPt", "Start point of base line for Koch curve", GH_ParamAccess.item);
            pManager.AddPointParameter("End point", "endPt", "End point of base line for Koch curve", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Koch curve", "kochCrv", "Koch curves", GH_ParamAccess.list);
        }
        #endregion

        //Method entry point
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Declare placeholder variable input.
            int num = 1;
            var startPt = new Point3d(double.NaN, double.NaN, double.NaN) ;
            var endPt = new Point3d(double.NaN, double.NaN, double.NaN);
            //Retrieve input data
            if(!DA.GetData(0, ref num)) { return; }
            if(!DA.GetData(1, ref startPt)) { return; }
            if(!DA.GetData(2, ref endPt)) { return; }

            //Set initial List of curves.
            var resultCurves = new List<Curve>();
            //call method
            RecursiveCurves(startPt, endPt, num, resultCurves);

            DA.SetDataList(0, resultCurves);
        }

        //Generate points for Koch curve.
        public List<Point3d> KochCurve(Point3d pt1, Point3d pt5)
        {
            var baseLine = new Line(pt1, pt5);
            var pt2 = baseLine.PointAt((double) 1 / 3);
            var pt3 = baseLine.PointAt((double) 1 / 2);
            var pt4 = baseLine.PointAt((double) 2 / 3);

            var dir = new Vector3d(pt5.X - pt1.X, pt5.Y - pt1.Y, 0);
            dir.Rotate(Math.PI / 2, Vector3d.ZAxis);

            pt3 += dir * Math.Sin(-60);

            return new List<Point3d>() { pt1, pt2, pt3, pt4, pt5 };
        }

        //Compute recursive for Koch curves.
        public List<Curve> RecursiveCurves(Point3d pt1, Point3d pt5, int num, List<Curve> curves)
        {
            if(num > 0)
            {
                var newPts = KochCurve(pt1, pt5);
                var crv = new PolylineCurve(new List<Point3d>() { newPts[0], newPts[1], newPts[2], newPts[3], newPts[4] });
                if(num == 1)
                {
                    curves.Add(crv);
                }

                RecursiveCurves(newPts[0], newPts[1], num - 1, curves);
                RecursiveCurves(newPts[1], newPts[2], num - 1, curves);
                RecursiveCurves(newPts[2], newPts[3], num - 1, curves);
                RecursiveCurves(newPts[3], newPts[4], num - 1, curves);
            }

            return curves;
        }


        protected override System.Drawing.Bitmap Icon => RecursiveCurveBasic.Properties.Resources.recursiveIcon;

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        public override Guid ComponentGuid => new Guid("{2FEA635E-F043-4C29-82E1-D669B6E47447}");
    }
}
