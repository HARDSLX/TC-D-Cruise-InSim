using System;
using System.Collections.Generic;
using System.Text;

namespace LFS_External_Client
{
    static class Dealer
    {
        static public int GetCarPrice(string CarName)
        {
            switch (CarName.ToUpper())
            {
                case "XFG": return 40000;
                case "XRG": return 40000;
                case "LX4": return 60000;

                case "LX6": return 111000;

                case "RB4": return 97000;
                case "FXO": return 80000;

                case "VWS": return 70670;
                case "XRT": return 85000;
                case "RAC": return 108000;
                case "FZ5": return 160000;

                case "UFR": return 185000;
                case "XFR": return 190000;
                case "FXR": return 300000;
                case "XRR": return 300000;
                case "FZR": return 300000;

                case "MRT": return 153500;
                case "FBM": return 660950;

            }
            return 0;
        }

        static public int GetCarValue(string CarName)
        {
            return (int)(GetCarPrice(CarName) * .70);
        }
    }
}
