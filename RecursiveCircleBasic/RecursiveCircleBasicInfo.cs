using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

namespace RecursiveCurveBasic
{
    public class RecursiveCircleBasicInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "RecursiveCircleBasic";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("254dcef2-4473-40c3-b82f-57cfaa3d7740");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "masayaTKT, meenaxydesign";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }

    public class MeenaxyCategoryIcon : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            Instances.ComponentServer.AddCategoryIcon("Meenaxy", Properties.Resources.meenaxyLogo);
            Instances.ComponentServer.AddCategorySymbolName("Meenaxy", 'M');
            return GH_LoadingInstruction.Proceed;
        }
    }
}
