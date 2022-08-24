using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace RecursiveCurveBasic
{
    /// <summary>
    /// Recursive Circle(alternative version)
    /// multi-threading component intherit from GH_TaskCapableComponent
    /// </summary>
    public class RecursiveCircleBasicTaskComponent : GH_TaskCapableComponent<RecursiveCircleBasicTaskComponent.SolveResults>
    {
        //Constructor
        public RecursiveCircleBasicTaskComponent() : base("RecursiveCircleBasic_Task", "RCB_t", "Multi-threading compute Recursive Circel", "Meenaxy", "Test")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Radius", "R", "Radius of the Origin Circle", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Level", "L", "Level of recursive depth", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddCircleParameter("Circles", "C", "Recursive circles", GH_ParamAccess.list);
        }

        ///<summary>
        ///separate method
        ///1. Collect input data
        ///2. Compute results on given data
        ///3. Set output data
        ///</summary>

        public class SolveResults
        {
            public List<Circle> Value { get; set; }
        }

        /// <summary>
        /// This is the method that create circle for recursive process
        /// Create a Compute function that takes the input retrieve from IGH_DataAccess
        /// and returns an instance of SolveResults
        /// </summary>

        private static SolveResults ComputeReCursiveCircles(double radius, int level)
        {
            var result = new SolveResults();

            //Set (initial) List of circles
            var circles = new List<Circle>();

            //Creat new circle and add a (intial) list
            var circle01 = new Circle(new Point3d(0, 0, 0), radius);
            circles.Add(circle01);

            //Set two list for recursive process
            var tempList01 = new List<Circle>();
            var tempList02 = new List<Circle>();
            tempList01 = circles;

            //Recursive loop
            int i = 0;
            while (i < level)
            {
                tempList02 = CreateRecursiveCircle(radius, tempList01);
                tempList01 = tempList02;
                circles.AddRange(tempList01);
                i++;
            }
            result.Value = circles;

            return result;
        }

        public static List<Circle> CreateRecursiveCircle(double radius, List<Circle> circles)
        {
            var newCircles = new List<Circle>();
            foreach (var circle in circles)
            {
                double newRadius = circle.Radius / 2;
                var newCircle = new Circle(new Point3d(newRadius, 0, 0), newRadius);

                newCircles.Add(newCircle);
            }

            return newCircles;
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Declare placeholder valuables for the input data
            double radius = double.NaN;
            int level = 1;
            //Retrieve input data
            if (!DA.GetData(0, ref radius)) { return; }
            if (!DA.GetData(1, ref level)) { return; }

            if (InPreSolve)
            {
                //Queue up the task
                Task<SolveResults> task = Task.Run(() => ComputeReCursiveCircles(radius, level), CancelToken);
                TaskList.Add(task);
                return;
            }

            if(!GetSolveResults(DA, out SolveResults result))
            {
                //Compute results on a given data
                result = ComputeReCursiveCircles(radius, level);
            }

            //Set output data
            if(result != null)
            {
                DA.SetDataList(0, result.Value);
            }
        }

        //Icon made by [https://www.flaticon.com/authors/those-icons] from www.flaticon.com
        protected override System.Drawing.Bitmap Icon => RecursiveCurveBasic.Properties.Resources.recursiveIcon;

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        public override Guid ComponentGuid => new Guid("{16D76F52-9038-4FA9-B771-E4DFB9122BC1}");

    }
}
