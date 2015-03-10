using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEAPDFPArser
{
    public enum WorkingArea
    {
        LivingRoom,
        Bedrooms,
        Kitchen,
        SelfService,
        Lighting,
        TextileRugs,
        CookingEating,
        IKEAFoodRestaurant,
        LogisticsCommon,
        StockControl,
        GoodsReceiving,
        GoodsReplenishment,
        FullServe,
        ComIn,
        Family,
        CustomerService,
        CSEntrance,
        CSTrolleyHandling,
        CashAdministration,
        Recovery,
        BuildingProjects
    }

    public static class WorkingAreaExtensions
    {
        public static string ToString(this WorkingArea wa)
        {
            switch (wa)
            {
                case WorkingArea.LivingRoom: return "Living Room";
                case WorkingArea.Bedrooms: return "Bedrooms";
                case WorkingArea.Kitchen: return "Kitchen";
                case WorkingArea.SelfService: return "Self Service";
                case WorkingArea.Lighting: return "Lighting";
                case WorkingArea.TextileRugs: return "Textiles & Rugs";
                case WorkingArea.CookingEating: return "Cooking & Eating";
                case WorkingArea.IKEAFoodRestaurant: return "IKEA Food Restaurant";
                case WorkingArea.LogisticsCommon: return "Logistics Common(TS)";
                case WorkingArea.StockControl: return "Stock Control";
                case WorkingArea.GoodsReceiving: return "Goods Receiving";
                case WorkingArea.GoodsReplenishment: return "Goods Replenishment";
                case WorkingArea.FullServe: return "Full Serve";
                case WorkingArea.ComIn: return "Com & In";
                case WorkingArea.Family: return "Family";
                case WorkingArea.CustomerService: return "Customer Service";
                case WorkingArea.CSEntrance: return "CS Entrance";
                case WorkingArea.CSTrolleyHandling: return "CS Trolley Handling";
                case WorkingArea.CashAdministration: return "Cash Administration";
                case WorkingArea.Recovery: return "Recovery";
                case WorkingArea.BuildingProjects: return "Building Projects";
                default: throw new ArgumentException("Unknown enum of type: WorkingArea.");
            }
        }
    }
}
