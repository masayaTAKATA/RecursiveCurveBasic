using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace RecursiveCurveBasic
{
    public class RecursiveCircleBasicComponent : GH_Component
    {
        //Constructor
        public RecursiveCircleBasicComponent() : base("RecursiveCircleBasic", "RCB", "Description", "User", "Default")
        {
        }

        //Input
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Radius", "R", "Radius of the Origin circle", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Level", "L", "Level of recursive depth", GH_ParamAccess.item);
        }

        //Output
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCircleParameter("Circles", "C", "Recursive circles", GH_ParamAccess.list);
        }

        //Method (Entry point)
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Declare placeholder variables for the input data.
            double radius = double.NaN;
            int level = 1;

            //Retrieve input data.
            if(!DA.GetData(0, ref radius)) { return; }
            if(!DA.GetData(1, ref level)) { return; }

            //Set a (initial) List of circles.
            var circles = new List<Circle>();

            //Create a new circle and add a (initial) List
            var circle01 = new Circle(new Point3d(0, 0, 0), radius);
            circles.Add(circle01);

            //Set two List for recursive process.
            var tempList01 = new List<Circle>();
            var tempList02 = new List<Circle>();
            tempList01 = circles;

            //Recursive loop.
            int i = 0;
            while(i < level)
            {
                tempList02 = CreateRecursiveCircle(radius, tempList01);
                tempList01 = tempList02;
                circles.AddRange(tempList02);
                i++;

            }

            DA.SetDataList(0, circles);
        }

        /// <summary>
        /// Method
        /// This is the method that create circles for recursive process.
        /// </summary>
        public List<Circle> CreateRecursiveCircle(double radius, List<Circle> circles)
        {
            var newCircles = new List<Circle>();
            foreach(Circle circle in circles)
            {
                double newRadius = circle.Radius / 2;
                var newCircle = new Circle(new Point3d(newRadius, 0, 0), newRadius);
                newCircles.Add(newCircle);

            }

            return newCircles;
  
        }

       

        //Icon made by [https://www.flaticon.com/authors/those-icons] from www.flaticon.com 
        protected override System.Drawing.Bitmap Icon => RecursiveCurveBasic.Properties.Resources.recursiveIcon;

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        public override Guid ComponentGuid => new Guid("{34E1E949-A9F6-48F4-A3C3-D0D6B972E0A6}");
    }
}
