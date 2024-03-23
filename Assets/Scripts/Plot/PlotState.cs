using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Plot
{
    public class PlotState
    {
        public bool isTilled { get; set; }
        public float growSpeed {  get; set; }
        public float timeBtwStageWithGrowRate { get; set; }
        public bool isPlanted { get; set; }
        public string plantName { get; set; }  
        public int plantState { get; set; }
        public int plotId {  get; set; } 
        public bool isPlantDie { get; set; }
        public bool isDry {  get; set; }
        public bool isBought {  get; set; }
    }
}
