using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace RecursiveCircleBasic
{
    public class RecursiveCircleBasicComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public RecursiveCircleBasicComponent()
          : base("RecursiveCircleBasic", "RecursiveCircleBasic",
              "Description",
              "User", "test")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Radius", "R", "Radius of the Origin circle", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Level", "L", "Level of recursive depth", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCircleParameter("Circles", "C", "Recursive circles", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Declare placeholder variables for the input data.
            double radius = double.NaN;
            int level = 1;

            //Retrieve input data.
            if(!DA.GetData(0, ref radius)) { return; }
            if(!DA.GetData(1, ref level)) { return; }

            //Set a (initial) List of circles.
            List<Circle> circles = new List<Circle>();

            //Create a new circle and add a (initial) List
            Circle circle01 = new Circle(new Point3d(0, 0, 0), radius);
            circles.Add(circle01);

            //Set two List for recursive process.
            List<Circle> tempList01 = new List<Circle>();
            List<Circle> tempList02 = new List<Circle>();
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
        /// This is the method that create circles for recursive process.
        /// </summary>
        public List<Circle> CreateRecursiveCircle(double radius, List<Circle> circles)
        {
            List<Circle> newCircles = new List<Circle>();
            foreach(Circle circle in circles)
            {
                double newRadius = circle.Radius / 2;
                Circle newCircle = new Circle(new Point3d(newRadius, 0, 0), newRadius);
                newCircles.Add(newCircle);

            }

            return newCircles;
  
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("b0445f3e-f7fe-4706-aeb9-646267eeb5c8"); }
        }
    }
}
