using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.DataService
{
    public class ScreenPara:MonoBehaviour
    {
        public static ScreenPara Instance;

        public bool isContinue;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void setIsContinue(bool isContinue)
        {
            this.isContinue = isContinue;
        }

    }
}
