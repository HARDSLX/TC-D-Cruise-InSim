using System;
using System.Windows.Forms;
using System.Collections.Generic;
using LFS_External;
using LFS_External.InSim;
using System.Threading;
using System.IO;
using System.Reflection;

namespace LFS_External_Client
{
    public partial class Form1
    {
        // MCI Update
        private void MCI_Update(int PLID)
        {
            try
            {
                #region ' UniqueID Loader '
                int IDX = -1;
                for (int i = 0; i < Connections.Count; i++)
                {
                    if (Connections[i].PlayerID == PLID)
                    {
                        IDX = i;
                        break;
                    }
                }
                if (IDX == -1)
                    return;
                clsConnection Conn = Connections[IDX];
                clsConnection ChaseCon = Connections[GetConnIdx(Connections[IDX].Chasee)];
                clsConnection TowCon = Connections[GetConnIdx(Connections[IDX].Towee)];
                #endregion

                #region ' Cruise '

                decimal SpeedMS = (decimal)(((Conn.CompCar.Speed / 32768f) * 100f) / 2);
                decimal Speed = (decimal)((Conn.CompCar.Speed * (100f / 32768f)) * 3.6f);

                Conn.TotalDistance += Convert.ToInt32(SpeedMS);
                Conn.TripMeter += Convert.ToInt32(SpeedMS);
                Conn.BonusDistance += Convert.ToInt32(SpeedMS);
                Conn.HealthDist += Convert.ToInt32(SpeedMS);

                #endregion

                #region ' CompCar Detailzzz '

                var kmh = Conn.CompCar.Speed / 91;
                var mph = Conn.CompCar.Speed / 146;
                var direction = Conn.CompCar.Direction / 180;
                var node = Conn.CompCar.Node;
                var pathx = Conn.CompCar.X / 196608;
                var pathy = Conn.CompCar.Y / 196608;
                var pathz = Conn.CompCar.Z / 98304;
                var angle = Conn.CompCar.AngVel / 30;
                string Car = Conn.CurrentCar;
                string anglenew = "";

                anglenew = angle.ToString().Replace("-", "");

                #endregion

                #region ' XY NODES CONSTRUCTION '
                /*
                if (Conn.IsAdmin == 1)
                {
                    InSim.Send_BTN_CreateButton("^1X: " + pathx + " ^2Y: " + pathy + " ^3Z: " + pathz, Flags.ButtonStyles.ISB_DARK, 5, 20, 50, 157, 100, (Conn.UniqueID), 2, false);
                    InSim.Send_BTN_CreateButton("^6Node: " + node, Flags.ButtonStyles.ISB_DARK, 5, 15, 55, 160, 102, (Conn.UniqueID), 2, false);   // Spare Button
                    InSim.Send_BTN_CreateButton("^5Direction: " + direction, Flags.ButtonStyles.ISB_DARK, 5, 15, 60, 160, 103, (Conn.UniqueID), 2, false);   // Spare Button
                    if (angle / 45 > 1)
                    {
                        InSim.Send_BTN_CreateButton("^2" + anglenew + "^Jß", Flags.ButtonStyles.ISB_DARK, 5, 15, 65, 160, 104, (Conn.UniqueID), 2, false);   // Spare Button
                    }
                    else
                    {
                        InSim.Send_BTN_CreateButton("^1" + anglenew + "^Jß", Flags.ButtonStyles.ISB_DARK, 5, 15, 65, 160, 104, (Conn.UniqueID), 2, false);   // Spare Button
                    }
                }
                */
                #endregion

                #region ' Track Info '

                switch (TrackName)
                {
                    case "KY1":
                        #region ' EVENT ' 
                        Conn.LocationBox = "^7K1 ^1EVENT ^3--- km/h";
                            InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^0"+Conn.CompCar.Speed / 91, Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                            Conn.Location = "EVENT";
                            Conn.LastSeen = "EVENT, Kyoto State";
                        #endregion
                        break;

                    case "KY3X":
                        #region ' Streets and Info '




                        #region ' Streets and Info '

                        //#region ' Safe Zone & Autobahn '
                        //if (Conn.CompCar.Node > 120 && Conn.CompCar.Node < 147)
                        //{
                        if (Conn.CompCar.X / 196608 >= -246 && Conn.CompCar.X / 196608 <= -234 && Conn.CompCar.Y / 196608 >= 228 && Conn.CompCar.Y / 196608 <= 237 || Conn.CompCar.X / 196608 >= -247 && Conn.CompCar.X / 196608 <= -233 && Conn.CompCar.Y / 196608 >= 219 && Conn.CompCar.Y / 196608 <= 228 || Conn.CompCar.X / 196608 >= -248 && Conn.CompCar.X / 196608 <= -237 && Conn.CompCar.Y / 196608 >= 210 && Conn.CompCar.Y / 196608 <= 219 || Conn.CompCar.X / 196608 >= -248 && Conn.CompCar.X / 196608 <= -237 && Conn.CompCar.Y / 196608 >= 201 && Conn.CompCar.Y / 196608 <= 210 || Conn.CompCar.X / 196608 >= -248 && Conn.CompCar.X / 196608 <= -237 && Conn.CompCar.Y / 196608 >= 186 && Conn.CompCar.Y / 196608 <= 201 || Conn.CompCar.X / 196608 >= -248 && Conn.CompCar.X / 196608 <= -236 && Conn.CompCar.Y / 196608 >= 179 && Conn.CompCar.Y / 196608 <= 186 || Conn.CompCar.X / 196608 >= -247 && Conn.CompCar.X / 196608 <= -236 && Conn.CompCar.Y / 196608 >= 164 && Conn.CompCar.Y / 196608 <= 179 || Conn.CompCar.X / 196608 >= -246 && Conn.CompCar.X / 196608 <= -234 && Conn.CompCar.Y / 196608 >= 155 && Conn.CompCar.Y / 196608 <= 164 || Conn.CompCar.X / 196608 >= -245 && Conn.CompCar.X / 196608 <= -234 && Conn.CompCar.Y / 196608 >= 143 && Conn.CompCar.Y / 196608 <= 155 || Conn.CompCar.X / 196608 >= -244 && Conn.CompCar.X / 196608 <= -232 && Conn.CompCar.Y / 196608 >= 135 && Conn.CompCar.Y / 196608 <= 143 || Conn.CompCar.X / 196608 >= -241 && Conn.CompCar.X / 196608 <= -235 && Conn.CompCar.Y / 196608 >= 125 && Conn.CompCar.Y / 196608 <= 135)
                        {

                            Conn.LocationBox = "^7K1 ^2Safe Zone ^380 km/h";
                            InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^1PIT", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                            Conn.Location = "Safe Zone";
                            Conn.LastSeen = "Safe Zone, Kyoto State";
                        }
                        if (Conn.CompCar.X / 196608 >= -246 && Conn.CompCar.X / 196608 <= -242 && Conn.CompCar.Y / 196608 >= 241 && Conn.CompCar.Y / 196608 <= 244)
                        {
                            if (Conn.CompCar.Direction / 180 <= 65 && Conn.CompCar.Direction / 180 >= 0 || Conn.CompCar.Direction / 180 <= 361 && Conn.CompCar.Direction / 180 >= 270)
                            {

                                Conn.LocationBox = "^7K2 Long Street ^3160 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0160", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Long Street";
                                Conn.LastSeen = "Long Street, Kyoto";

                            }
                        }
                        #region ' Small Corner, Long Street '
                        if (Conn.CompCar.X / 196608 <= -175 && Conn.CompCar.X / 196608 >= -185 && Conn.CompCar.Y / 196608 >= 325 && Conn.CompCar.Y / 196608 <= 350)
                        {
                            if (Conn.CompCar.Direction / 180 <= 361 && Conn.CompCar.Direction / 180 >= 200)
                            {

                                Conn.LocationBox = "^7K3 Small Corner ^380 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^080", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Small Corner";
                                Conn.LastSeen = "Small Corner, Kyoto";

                            }
                            else if (Conn.CompCar.Direction / 180 <= 199 && Conn.CompCar.Direction / 180 >= 0)
                            {

                                Conn.LocationBox = "^7K2 Long Street ^3160 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0160", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Long Street";
                                Conn.LastSeen = "Long Street, Kyoto";

                            }
                        }
                        #endregion

                        #region ' Small Corner, Down Road '
                        if (Conn.CompCar.X / 196608 <= -107 && Conn.CompCar.X / 196608 >= -111 && Conn.CompCar.Y / 196608 >= 321 && Conn.CompCar.Y / 196608 <= 334)
                        {
                            if (Conn.CompCar.Direction / 180 <= 362 && Conn.CompCar.Direction / 180 >= 250 || Conn.CompCar.Direction / 180 <= 20 && Conn.CompCar.Direction / 180 >= 0)
                            {

                                Conn.LocationBox = "^7K3 Small Corner ^380 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^080", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Small Corner";
                                Conn.LastSeen = "Small Corner, Kyoto";

                            }
                            else if (Conn.CompCar.Direction / 180 <= 249 && Conn.CompCar.Direction / 180 >= 21)
                            {

                                Conn.LocationBox = "^7K4 Down Road ^3120 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0120", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Down Road";
                                Conn.LastSeen = "Down Road, Kyoto";

                            }
                        }
                        #endregion

                        #region ' Down Road, Second Road '
                        if (Conn.CompCar.X / 196608 <= -186 && Conn.CompCar.X / 196608 >= -200 && Conn.CompCar.Y / 196608 >= 263 && Conn.CompCar.Y / 196608 <= 268)
                        {
                            if (Conn.CompCar.Direction / 180 <= 230 && Conn.CompCar.Direction / 180 >= 65)
                            {

                                Conn.LocationBox = "^7K5 Second Road ^3120 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0120", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Second Road";
                                Conn.LastSeen = "Second Road, Kyoto";

                            }
                            else if (Conn.CompCar.Direction / 180 >= 231 && Conn.CompCar.Direction / 180 <= 361 || Conn.CompCar.Direction / 180 >= 0 && Conn.CompCar.Direction / 180 <= 64)
                            {

                                Conn.LocationBox = "^7K4 Down Road ^3120 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0120", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Down Road";
                                Conn.LastSeen = "Down Road, Kyoto";

                            }
                        }
                        #endregion

                        #region ' Kruising Autobahn, Second Road '
                        if (Conn.CompCar.X / 196608 >= -169 && Conn.CompCar.X / 196608 <= -163 && Conn.CompCar.Y / 196608 >= 151 && Conn.CompCar.Y / 196608 <= 179)
                        {
                            if (Conn.CompCar.Direction / 180 <= 250 && Conn.CompCar.Direction / 180 >= 170 || Conn.CompCar.Direction / 180 >= 250 && Conn.CompCar.Direction / 180 <= 300)
                            {

                                Conn.LocationBox = "^7K6 Highway1 ^3--- kmh";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0N/A", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Autobahn";
                                Conn.LastSeen = "Autobahn, Kyoto";

                            }
                            else if (Conn.CompCar.Direction / 180 >= 100 && Conn.CompCar.Direction / 180 <= 180 || Conn.CompCar.Direction / 180 >= 0 && Conn.CompCar.Direction / 180 <= 64)
                            {

                                Conn.LocationBox = "^7K5 Second Road ^3120 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0120", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Second Road";
                                Conn.LastSeen = "Second Road, Kyoto";

                            }
                        }
                        #endregion

                        #region ' Kruising Autobahn, Second Road 2 '
                        if (Conn.CompCar.X / 196608 >= -164 && Conn.CompCar.X / 196608 <= -157 && Conn.CompCar.Y / 196608 >= 100 && Conn.CompCar.Y / 196608 <= 124)
                        {
                            if (Conn.CompCar.Direction / 180 <= 70 && Conn.CompCar.Direction / 180 >= 0 || Conn.CompCar.Direction / 180 >= 290 && Conn.CompCar.Direction / 180 <= 362)
                            {

                                Conn.LocationBox = "^7K6 Highway1 ^3--- kmh";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0N/A", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Autobahn";
                                Conn.LastSeen = "Autobahn, Kyoto";

                            }
                            else if (Conn.CompCar.Direction / 180 >= 100 && Conn.CompCar.Direction / 180 <= 260)
                            {

                                Conn.LocationBox = "^7K6 Highway1 ^3--- kmh";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0N/A", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Autobahn";
                                Conn.LastSeen = "Autobahn, Kyoto";

                            }
                        }
                        #endregion

                        #region ' Kruising Autobahn, Drifty Corner, Central Roads '
                        if (Conn.CompCar.X / 196608 >= 29 && Conn.CompCar.X / 196608 <= 86 && Conn.CompCar.Y / 196608 >= 14 && Conn.CompCar.Y / 196608 <= 86)
                        {

                            Conn.LocationBox = "^7K8 Rounding ^380 km/h";
                            InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^080", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                            Conn.Location = "Rounding";
                            Conn.LastSeen = "Rounding, Kyoto";

                        }
                        #endregion

                        #region ' Rounding, Autobahn '
                        if (Conn.CompCar.X / 196608 <= 30 && Conn.CompCar.X / 196608 >= 29 && Conn.CompCar.Y / 196608 >= -12 && Conn.CompCar.Y / 196608 <= 13)
                        {
                            if (Conn.CompCar.Direction / 180 <= 230 && Conn.CompCar.Direction / 180 >= 70)
                            {

                                Conn.LocationBox = "^7K6 Highway1 ^3--- kmh";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0N/A", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Autobahn";
                                Conn.LastSeen = "Autobahn, Kyoto";

                            }
                        }
                        #endregion

                        #region ' Rounding, Autobahn '
                        if (Conn.CompCar.X / 196608 <= 88 && Conn.CompCar.X / 196608 >= 72 && Conn.CompCar.Y / 196608 >= 89 && Conn.CompCar.Y / 196608 <= 98)
                        {
                            if (Conn.CompCar.Direction / 180 <= 40 && Conn.CompCar.Direction / 180 >= 0 || Conn.CompCar.Direction / 180 <= 362 && Conn.CompCar.Direction / 180 >= 250)
                            {

                                Conn.LocationBox = "^7K6 Highway1 ^3--- kmh";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0N/A", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Autobahn";
                                Conn.LastSeen = "Autobahn, Kyoto";
                            }
                        }
                        #endregion

                        #region ' Rounding, Central Roads '
                        if (Conn.CompCar.X / 196608 <= 40 && Conn.CompCar.X / 196608 >= 27 && Conn.CompCar.Y / 196608 >= 78 && Conn.CompCar.Y / 196608 <= 102)
                        {
                            if (Conn.CompCar.Direction / 180 <= 140 && Conn.CompCar.Direction / 180 >= 0 || Conn.CompCar.Direction / 180 <= 362 && Conn.CompCar.Direction / 180 >= 340)
                            {

                                Conn.LocationBox = "^7K7 Central Roads ^3120 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0120", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Central Roads";
                                Conn.LastSeen = "Central Roads, Kyoto";

                            }
                        }
                        #endregion

                        #region ' Rounding, Slippy Corner '
                        if (Conn.CompCar.X / 196608 <= 95 && Conn.CompCar.X / 196608 >= 88 && Conn.CompCar.Y / 196608 >= 32 && Conn.CompCar.Y / 196608 <= 44)
                        {
                            if (Conn.CompCar.Direction / 180 >= 190 && Conn.CompCar.Direction / 180 <= 330)
                            {

                                Conn.LocationBox = "^7K7 Slippy Corner ^3120 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0120", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Slippy Corner";
                                Conn.LastSeen = "Slippy Corner, Kyoto";

                            }
                            if (Conn.CompCar.Direction / 180 >= 5 && Conn.CompCar.Direction / 180 <= 143)
                            {
                                Conn.LocationBox = "^7K8 Rounding ^380 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^080", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 185, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Rounding";
                                Conn.LastSeen = "Rounding, Kyoto";

                            }
                        }
                        #endregion

                        #region ' Autobahn, Central Roads '
                        if (Conn.CompCar.X / 196608 <= 67 && Conn.CompCar.X / 196608 >= 43 && Conn.CompCar.Y / 196608 >= 176 && Conn.CompCar.Y / 196608 <= 192)
                        {
                            if (Conn.CompCar.Direction / 180 <= 220 && Conn.CompCar.Direction / 180 >= 80)
                            {

                                Conn.LocationBox = "^7K9 Central Roads ^3120 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0120", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Central Roads";
                                Conn.LastSeen = "Central Roads, Kyoto";

                            }
                            else if (Conn.CompCar.Direction / 180 <= 362 && Conn.CompCar.Direction / 180 >= 221 || Conn.CompCar.Direction / 180 <= 79 && Conn.CompCar.Direction / 180 >= 0)
                            {
                                Conn.LocationBox = "^7K6 Highway1 ^3--- kmh";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0N/A", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Autobahn";
                                Conn.LastSeen = "Autobahn, Kyoto";

                            }
                        }
                        #endregion

                        #region ' Northern Roads, Slippy Corner '
                        if (Conn.CompCar.X / 196608 <= 78 && Conn.CompCar.X / 196608 >= 73 && Conn.CompCar.Y / 196608 >= -56 && Conn.CompCar.Y / 196608 <= -49)
                        {
                            if (Conn.CompCar.Direction / 180 >= 0 && Conn.CompCar.Direction / 180 <= 10 || Conn.CompCar.Direction / 180 >= 220 && Conn.CompCar.Direction / 180 <= 363)
                            {

                                Conn.LocationBox = "^7K7 Slippy Corner ^3120 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0120", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Slippy Corner";
                                Conn.LastSeen = "Slippy Corner, Kyoto";

                            }
                            if (Conn.CompCar.Direction / 180 >= 40 && Conn.CompCar.Direction / 180 <= 200)
                            {


                                Conn.LocationBox = "^7K7 Slippy Corner ^3120 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0120", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Northern Roads";
                                Conn.LastSeen = "Northern Roads, Kyoto";

                            }
                        }
                        #endregion

                        #region ' Northern Roads, Long Street '
                        if (Conn.CompCar.X / 196608 <= -136 && Conn.CompCar.X / 196608 >= -153 && Conn.CompCar.Y / 196608 >= -42 && Conn.CompCar.Y / 196608 <= -36)
                        {
                            if (Conn.CompCar.Direction / 180 >= 0 && Conn.CompCar.Direction / 180 <= 130 || Conn.CompCar.Direction / 180 >= 340 && Conn.CompCar.Direction / 180 <= 363)
                            {

                                Conn.LocationBox = "^7K11 Long Street ^3160 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0160", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Long Street";
                                Conn.LastSeen = "Long Street, Kyoto";

                            }
                            if (Conn.CompCar.Direction / 180 >= 150 && Conn.CompCar.Direction / 180 <= 300)
                            {

                                Conn.LocationBox = "^7K10 Northern Roads ^3120 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0120", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Northern Roads";
                                Conn.LastSeen = "Northern Roads, Kyoto";

                            }
                        }
                        #endregion

                        #region ' Northern Roads, Paid Parking '
                        if (Conn.CompCar.X / 196608 <= 70 && Conn.CompCar.X / 196608 >= 69 && Conn.CompCar.Y / 196608 >= -157 && Conn.CompCar.Y / 196608 <= -157)
                        {

                            if (Conn.CompCar.Direction / 180 >= 320 && Conn.CompCar.Direction / 180 <= 361 && Conn.CompCar.Direction / 180 >= 0 && Conn.CompCar.Direction / 180 <= 100)
                            {

                                Conn.LocationBox = "^7K10 Northern Roads ^3120 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0120", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Northern Roads";
                                Conn.LastSeen = "Northern Roads, Kyoto";
                            }
                        }
                        #endregion

                        #region ' Northern Roads, Paid Parking '
                        if (Conn.CompCar.X / 196608 <= 72 && Conn.CompCar.X / 196608 >= 67 && Conn.CompCar.Y / 196608 <= -149 && Conn.CompCar.Y / 196608 >= -155)
                        {
                            {

                                Conn.LocationBox = "^7K10 Northern Roads ^3120 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0120", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Northern Roads";
                                Conn.LastSeen = "Northern Roads, Kyoto";
                            }
                        }
                        #endregion

                        #region ' Northern Roads, Paid Parking '
                        if (Conn.CompCar.X / 196608 <= 39 && Conn.CompCar.X / 196608 >= 35 && Conn.CompCar.Y / 196608 <= -163 && Conn.CompCar.Y / 196608 >= -175)
                        {
                            {

                                Conn.LocationBox = "^7K10 Northern Roads ^3120 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0120", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Northern Roads";
                                Conn.LastSeen = "Northern Roads, Kyoto";
                            }
                        }
                        #endregion

                        #region   'RaceZone'

                        if (Conn.CompCar.X / 196608 >= -11 && Conn.CompCar.X / 196608 <= 70 && Conn.CompCar.Y / 196608 >= 52 && Conn.CompCar.Y / 196608 <= 111)
                        {



                            Conn.LocationBox = "^7K13 Race Zone ^3"+Conn.CompCar.Speed / 91+ " km/h";
                            InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^0---", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                            Conn.Location = "^CRace Zone";
                            Conn.LastSeen = "^CRace Zone";
                        }



                        if (Conn.CompCar.X / 196608 >= -48 && Conn.CompCar.X / 196608 <= -6 && Conn.CompCar.Y / 196608 >= -5 && Conn.CompCar.Y / 196608 <= 74)
                        {

                            Conn.LocationBox = "^7K13 Race Zone ^3" + Conn.CompCar.Speed / 91 + " km/h";
                            InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^0---", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                            Conn.Location = "^CRace Zone";
                            Conn.LastSeen = "^CRace Zone";
                        }

                        if (Conn.CompCar.X / 196608 >= -69 && Conn.CompCar.X / 196608 <= -27 && Conn.CompCar.Y / 196608 >= -31 && Conn.CompCar.Y / 196608 <= 2)
                        {

                            Conn.LocationBox = "^7K13 Race Zone ^3" + Conn.CompCar.Speed / 91 + " km/h";
                            InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^0---", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                            Conn.Location = "^CRace Zone";
                            Conn.LastSeen = "^CRace Zone";
                        }

                        if (Conn.CompCar.X / 196608 >= -74 && Conn.CompCar.X / 196608 <= -61 && Conn.CompCar.Y / 196608 >= -33 && Conn.CompCar.Y / 196608 <= -13)
                        {

                            Conn.LocationBox = "^7K13 Race Zone ^3" + Conn.CompCar.Speed / 91 + " km/h";
                            InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^0---", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                            Conn.Location = "^CRace Zone";
                            Conn.LastSeen = "^CRace Zone";
                        }

                        if (Conn.CompCar.X / 196608 >= -83 && Conn.CompCar.X / 196608 <= -73 && Conn.CompCar.Y / 196608 >= -33 && Conn.CompCar.Y / 196608 <= -11)
                        {

                            Conn.LocationBox = "^7K13 Race Zone ^3" + Conn.CompCar.Speed / 91 + " km/h";
                            InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^0---", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                            Conn.Location = "^CRace Zone";
                            Conn.LastSeen = "^CRace Zone";
                        }


                        #endregion

                        #region ' Paid Parking '
                       
                        #region Paid Parking
                        if (Conn.CompCar.X / 196608 >= 45 && Conn.CompCar.X / 196608 <= 47 && Conn.CompCar.Y / 196608 <= -167 && Conn.CompCar.Y / 196608 >= -180)
                        {
                            Conn.LocationBox = "^7K12 Parking ^330 km/h";
                            Conn.Location = "Parking";
                            Conn.LastSeen = "Parking, Kyoto";  

                        }
                        
                        
                        #endregion
                       
                        #endregion

                        

                        #endregion




                #endregion
                        break;

                    case "WE1X":
                        #region ' Location and Speed Limit '
                        #region ' Westhill way '
                        {
                            if (pathx >= -62 && pathx <= 41 && pathy >= 194 && pathy <= 212)
                            {
                                

                                Conn.LocationBox = "^7L2 ^3 Gas Station 40 km/h ";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^040", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Gas Station";
                                Conn.LastSeen = "Gas Station";
                            } 
                            else if (pathx >= -68 && pathx <= -64 && pathy >= 78 && pathy <= 186)
                            {
                                

                                Conn.LocationBox = "^7W1 ^2Safe Zone ^380 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^1PIT", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Safe Zone";
                                Conn.LastSeen = "Safe Zone";

                            }
                            else if (pathx >= -78 && pathx <= -74 && pathy >= -395 && pathy <= -390)
                            {
                                #region ' Speedlimit 140kmh/99ph '
                                if (kmh > 50)
                                {
                                    Conn.SpeedBox = "^1" + kmh + " kmh / 50 kmh";
                                }
                                else
                                {
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 50 kmh";
                                    }
                                }
                                #endregion

                                Conn.LocationBox = "^3Drag Zone Entry";
                                Conn.Location = "Drag Zone Entry";
                                Conn.LastSeen = "Drag Zone Entry";
                            }
                            else if (pathx >= -63 && pathx <= -58 && pathy >= 84 && pathy <= 183)
                            {
                                Conn.LocationBox = "^7W1 ^2Safe Zone ^380 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^1PIT", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Safe Zone";
                                Conn.LastSeen = "Safe Zone";

                            }
                            else if (pathx >= -81 && pathx <= 108 && pathy >= -426 && pathy <= -395)
                            {
                                Conn.LocationBox = "^7M2 ^2Drag Zone ^3--- km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0N/A", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Drag Zone";
                                Conn.LastSeen = "Drag Zone";
                                
                            }
                            else if (pathx >= -216 && pathx <= -204 && pathy >= -56 && pathy <= -27)
                            {


                                Conn.LocationBox = "^7M16 ^7Under The Bridge ^380 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^080", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Spawn Zone";
                                Conn.LastSeen = "Spawn Zone";

                            }
                            else if (pathx >= -85 && pathx <= -61 && pathy >= 263 && pathy <= 274 && pathz >= 2 && pathz <= 2)
                            {
                                Conn.LocationBox = "^7M16 ^7Under The Bridge ^380 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^080", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Under The Bridge";
                                Conn.LastSeen = "Under The Bridge";

                            }
                            else if (pathx >= -62 && pathx <= -55 && pathy >= 272 && pathy <= 276)
                            { // ot drygata strana
                                Conn.LocationBox = "^7M16 ^7Under The Bridge ^380 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^080", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Under The Bridge";
                                Conn.LastSeen = "Under The Bridge";

                            }
                            else if (pathx >= -93 && pathx <= -85 && pathy >= 260 && pathy <= 265)
                            { 
                                Conn.LocationBox = "^7M16 ^7Under The Bridge ^380 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^080", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Under The Bridge";
                                Conn.LastSeen = "Under The Bridge";

                            }

                            else if (pathx >= -233 && pathx <= -216 && pathy >= -69 && pathy <= -26)
                            {


                                Conn.LocationBox = "^7M4 ^2Spawn Zone ^380 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^080", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Spawn Zone";
                                Conn.LastSeen = "Spawn Zone";
                            }
                            else if (pathx >= -232 && pathx <= -207 && pathy >= 276 && pathy <= 301)
                            {
                                

                                Conn.LocationBox = "^7L6 ^7Parking ^350 km/h";
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^4•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7P", Flags.ButtonStyles.ISB_C1, 6, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Parking";
                                Conn.LastSeen = "Parking";
                            }
                            else if (pathx >= -212 && pathx <= -197 && pathy >= 268 && pathy <= 282)
                            {


                                Conn.LocationBox = "^7L5 ^7Parking Entry ^330 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^030", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Parking Entry";
                                Conn.LastSeen = "Parking Entry";
                            }
                            else if (pathx >= -200 && pathx <= -181 && pathy >= 260 && pathy <= 272)
                            {


                                Conn.LocationBox = "^7L5 ^7Parking Entry ^330 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^030", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Parking Entry";
                                Conn.LastSeen = "Parking Entry";
                            }
                            else if (pathx >= -136 && pathx <= -113 && pathy >= 84 && pathy <= 187)
                            {

                                Conn.LocationBox = "^3Ramp Area";
                                Conn.Location = "Ramp Area";
                                Conn.LastSeen = "Ramp Area";
                            }
                            else
                            {
                                Conn.LocationBox = "^7W6 ^7Westhill ^3110 km/h";
                               InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                               InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                               InSim.Send_BTN_CreateButton("^0110", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Westhill";
                                Conn.LastSeen = "Westhill";
                                
                            }
                        }
                        #endregion


                        #endregion
                        break;

                    case "FE4X":
                        #region ' Streets and Info '
                                #region 'servisa stacija REAL'

                                if (((Conn.CompCar.X / 65536) <= 213) && ((Conn.CompCar.X / 65536) >= 176) && ((Conn.CompCar.Y / 65536) <= 177) && ((Conn.CompCar.Y / 65536) >= 6))
                                {
                                    Conn.LocationBox = "^7J1 ^2Safe Zone ^380 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1PIT", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "Safe Zone";
                                    Conn.LastSeen = "Safe Zone, FE4X";
                                    
                                }
                                #endregion
                                #region '^7Misty Falls'

                                if (((Conn.CompCar.X / 65536) <= 216) && ((Conn.CompCar.X / 65536) >= 153) && ((Conn.CompCar.Y / 65536) <= 1) && ((Conn.CompCar.Y / 65536) >= -178))
                                {
                                    Conn.LocationBox = "^7J2 Misty Falls ^390 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^090", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "Misty Falls";
                                    Conn.LastSeen = "^9^7Misty Falls,  Fernbay Open";
                                }

                                #endregion
                                #region 'Пътят е в ремонт'

                                if (((Conn.CompCar.X / 65536) <= 272) && ((Conn.CompCar.X / 65536) >= 160) && ((Conn.CompCar.Y / 65536) <= -180) && ((Conn.CompCar.Y / 65536) >= -261))
                                {
                                    Conn.LocationBox = "^7J3 Racer's Street ^390 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^090", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "^7Racer's Street";
                                    Conn.LastSeen = "^7Racer's Street,  Fernbay Open";
                                }

                                #endregion
                                #region 'Пътят е в ремонт'

                                if (((Conn.CompCar.X / 65536) <= 340) && ((Conn.CompCar.X / 65536) >= 159) && ((Conn.CompCar.Y / 65536) <= -270) && ((Conn.CompCar.Y / 65536) >= -503))
                                {
                                    Conn.LocationBox = "^7J3 Racer's Street ^390 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^090", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "^7Racer's Street";
                                    Conn.LastSeen = "^7Racer's Street,  Fernbay Open";
                                }

                                #endregion
                                #region 'Fast Street'

                                if (((Conn.CompCar.X / 65536) <= 208) && ((Conn.CompCar.X / 65536) >= 139) && ((Conn.CompCar.Y / 65536) <= -647) && ((Conn.CompCar.Y / 65536) >= -833))
                                {
                                    Conn.LocationBox = "^7J4 ^7Fast Street ^3100 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^0100", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "^9Fast Street";
                                    Conn.LastSeen = "^9Fast Street,  Fernbay Open";
                                }

                                #endregion
                                #region 'Старият път'

                                if (((Conn.CompCar.X / 65536) <= 193) && ((Conn.CompCar.X / 65536) >= -45) && ((Conn.CompCar.Y / 65536) <= -821) && ((Conn.CompCar.Y / 65536) >= -942))
                                {
                                    Conn.LocationBox = "^7J5 ^7Old Road ^3100 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^0100", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "^7Old Road";
                                    Conn.LastSeen = "^7Old Road,  Fernbay Open";
                                }

                                #endregion
                                #region 'Одрин'

                                if (((Conn.CompCar.X / 65536) <= 10) && ((Conn.CompCar.X / 65536) >= -84) && ((Conn.CompCar.Y / 65536) <= -702) && ((Conn.CompCar.Y / 65536) >= -849))
                                {
                                    Conn.LocationBox = "^7J5 ^7Old Road ^3100 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^0100", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "^7Old Road";
                                    Conn.LastSeen = "^7Old Road,  Fernbay Open";
                                }

                                #endregion
                                #region 'Аспарухово'

                                if (((Conn.CompCar.X / 65536) <= 19) && ((Conn.CompCar.X / 65536) >= -59) && ((Conn.CompCar.Y / 65536) <= -246) && ((Conn.CompCar.Y / 65536) >= -540))
                                {
                                    Conn.LocationBox = "^7J6 Jack's Street ^3130 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^0130", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "^7Jack's Street";
                                    Conn.LastSeen = "^7Jack's Street,  Fernbay Open";
                                    
                                }

                                #endregion
                                #region 'Хладилник'

                                if (((Conn.CompCar.X / 65536) <= 60) && ((Conn.CompCar.X / 65536) >= -30) && ((Conn.CompCar.Y / 65536) <= 37) && ((Conn.CompCar.Y / 65536) >= -231))
                                {
                                    Conn.LocationBox = "^7J6 Ice Road ^390 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^090", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "^7Ice Road";
                                    Conn.LastSeen = "^7Ice Road, Fernbay Open";
                                }

                                #endregion
                                #region 'Янко Янев'

                                if (((Conn.CompCar.X / 65536) <= 60) && ((Conn.CompCar.X / 65536) >= -43) && ((Conn.CompCar.Y / 65536) <= 103) && ((Conn.CompCar.Y / 65536) >= 42))
                                {
                                    Conn.LocationBox = "^7J7 Best Street ^390 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^090", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "^3Best ^4Street";
                                    Conn.LastSeen = "^3Best ^4Street, Fernbay Open";
                                }

                                #endregion
                                #region 'Роза'

                                if (((Conn.CompCar.X / 65536) <= 52) && ((Conn.CompCar.X / 65536) >= -7) && ((Conn.CompCar.Y / 65536) <= 431) && ((Conn.CompCar.Y / 65536) >= 114))
                                {
                                    Conn.LocationBox = "^7J8 Sun Rice ^3160 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^0160", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "^3Sun ^6Rice";
                                    Conn.LastSeen = "^3Sun ^6Rice, Fernbay Open";
                                }

                                #endregion
                                #region 'Бачо Киро'

                                if (((Conn.CompCar.X / 65536) <= 48) && ((Conn.CompCar.X / 65536) >= -382) && ((Conn.CompCar.Y / 65536) <= 574) && ((Conn.CompCar.Y / 65536) >= 264))
                                {
                                    Conn.LocationBox = "^7J9 Master Driver Street ^390 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^090", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "^3Master ^0Driver's ^7Street";
                                    Conn.LastSeen = "^3Master ^0Driver's ^7Street, Fernbay Open";
                                }

                                #endregion
                                #region 'Мистериозният път'

                                if (((Conn.CompCar.X / 65536) <= -160) && ((Conn.CompCar.X / 65536) >= -281) && ((Conn.CompCar.Y / 65536) <= 258) && ((Conn.CompCar.Y / 65536) >= -49))
                                {
                                    Conn.LocationBox = "^7J10 Mysterious Road ^3120 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^0120", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "^7Mysterious Road";
                                    Conn.LastSeen = "^7Mysterious Road, Fernbay Open";
                                }

                                #endregion
                                #region 'Стара Планина'

                                if (((Conn.CompCar.X / 65536) <= -166) && ((Conn.CompCar.X / 65536) >= -610) && ((Conn.CompCar.Y / 65536) <= -53) && ((Conn.CompCar.Y / 65536) >= -405))
                                {
                                    Conn.LocationBox = "^7J11 Old Mountain ^390 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^090", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "^7Old Mountain";
                                    Conn.LastSeen = "^7Old Mountain, Fernbay Open";
                                }

                                #endregion
                                #region '3-ти Март'
                                if (((Conn.CompCar.X / 65536) <= -272) && ((Conn.CompCar.X / 65536) >= -520) && ((Conn.CompCar.Y / 65536) <= -403) && ((Conn.CompCar.Y / 65536) >= -773))
                                {
                                    Conn.LocationBox = "^7J12 Drifter's Turn ^390 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^090", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "^3Drifter's ^6Turns";
                                    Conn.LastSeen = "^3Drifter's ^6Turns, Fernbay Open";
                                }

                                #endregion
                                #region 'Автомагистрала-Хемус'

                                if (((Conn.CompCar.X / 65536) <= 417) && ((Conn.CompCar.X / 65536) >= -357) && ((Conn.CompCar.Y / 65536) <= -542) && ((Conn.CompCar.Y / 65536) >= -780))
                                {
                                    Conn.LocationBox = "^7J13 Bridge Highway ^3140 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^0140", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "^7Bridge Highway";
                                    Conn.LastSeen = "^7Bridge Highway, Fernbay Open";
                                }

                                #endregion
                                #region ' Dangerous Turns '

                                if (((Conn.CompCar.X / 65536) <= 606) && ((Conn.CompCar.X / 65536) >= 370) && ((Conn.CompCar.Y / 65536) <= -206) && ((Conn.CompCar.Y / 65536) >= -560))
                                {
                                    Conn.LocationBox = "^7J14 Dangerous Turns ^3100 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^0100", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "^9Dangerous Turns";
                                    Conn.LastSeen = "^9Dangerous Turns, Fernbay Open";
                                }

                                #endregion
                                #region ' Sunny Street '

                                if (((Conn.CompCar.X / 65536) <= 587) && ((Conn.CompCar.X / 65536) >= 306) && ((Conn.CompCar.Y / 65536) <= 9) && ((Conn.CompCar.Y / 65536) >= -232))
                                {
                                    Conn.LocationBox = "^7J15 Sunny Street ^390 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^090", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "^9Sunny Street";
                                    Conn.LastSeen = "^9Sunny Street, Fernbay Open";
                                }

                                #endregion
                                #region 'Старият път'

                                if (((Conn.CompCar.X / 65536) <= 370) && ((Conn.CompCar.X / 65536) >= 127) && ((Conn.CompCar.Y / 65536) <= 509) && ((Conn.CompCar.Y / 65536) >= 200))
                                {
                                    Conn.LocationBox = "^7J16 Old Road ^380 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^080", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "^7Old Road";
                                    Conn.LastSeen = "^7Old Road, Fernbay Open";
                                }

                                #endregion
                                #region 'Offroad'

                                if (((Conn.CompCar.X / 65536) <= -4) && ((Conn.CompCar.X / 65536) >= -175) && ((Conn.CompCar.Y / 65536) <= 51) && ((Conn.CompCar.Y / 65536) >= -168))
                                {
                                    Conn.LocationBox = "^7J17 Old Road ^3--- km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^0N/A", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "Offroad";
                                    Conn.LastSeen = "Offroad, Fernbay Open";
                                }

                                #endregion
                                #region 'Offroad2'

                                if (((Conn.CompCar.X / 65536) <= -67) && ((Conn.CompCar.X / 65536) >= -317) && ((Conn.CompCar.Y / 65536) <= -212) && ((Conn.CompCar.Y / 65536) >= -514))
                                {
                                    Conn.LocationBox = "^7J18 Old Road ^3--- km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^0N/A", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "Offroad";
                                    Conn.LastSeen = "Offroad, Fernbay Open";
                                }

                                #endregion
                                #region 'Highway'

                                if (((Conn.CompCar.X / 65536) <= 171) && ((Conn.CompCar.X / 65536) >= 106) && ((Conn.CompCar.Y / 65536) <= 194) && ((Conn.CompCar.Y / 65536) >= 12))
                                {
                                    Conn.LocationBox = "^7J19 Highway ^3120 km/h";
                                    InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^0120", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                    Conn.Location = "Highway";
                                    Conn.LastSeen = "^7Highway, Fernbay Open";
                                }

                                #endregion


                                #endregion
                        break;
                    case "RO1X":

                        #region ' RO1X '
                        {
                            if (pathx >= -96 && pathx <= -56 && pathy >= -67 && pathy <= 33)
                            {
                                

                                Conn.LocationBox = "^3R1 ^2Safe Zone";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^1PIT", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Pit Station";
                                Conn.LastSeen = "Pit Station";
                            }
                            else if (pathx >= -57 && pathx <= -40 && pathy >= -62 && pathy <= -49)
                            {
                                

                                Conn.LocationBox = "^3R2 ^2Gas Station";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^035", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Gas Station";
                                Conn.LastSeen = "Gas Station";
                            }
                            else if (pathx >= -10 && pathx <= -5 && pathy >= -136 && pathy <= -98 && pathz >= 0 && pathz <= 0)
                            {
                                


                                Conn.LocationBox = "^3R3 ^7Under the Bridge";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^080", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Payed Zone";
                                Conn.LastSeen = "Payed Zone";
                            }
                            else if (pathx >= -11 && pathx <= -8 && pathy >= -98 && pathy <= -92)
                            {


                                Conn.LocationBox = "^3R3 ^7Under the Bridge";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^080", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Paying Zone";
                                Conn.LastSeen = "Paying Zone";
                            }
                            else if (pathx >= -8 && pathx <= -5 && pathy >= -142 && pathy <= -136)
                            {


                                Conn.LocationBox = "^3R3 ^7Under the Bridge";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^080", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Paying Zone";
                                Conn.LastSeen = "Paying Zone";
                            }
                            else if (pathx >= -11 && pathx <= 8 && pathy >= 7 && pathy <= 44)
                            {
                                

                                Conn.LocationBox = "^3R4 ^2Parking";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^040", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "^2Parking";
                                Conn.LastSeen = "^2Parking";
                            }
                            else
                            {
                                
                                Conn.LocationBox = "^3R5 ^7Rockingham City";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0135", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Rockingham City";
                                Conn.LastSeen = "Rockingham City";
                                //Conn.Bilet = 0;
                            }
                        }
                        #endregion

                        break;
                    case "SO4X":

                        #region ' SO4X '
                        {
                            //X169 Y209 X247 Y41
                            if (((Conn.CompCar.X / 65536) <= 247) && ((Conn.CompCar.X / 65536) >= 169) && ((Conn.CompCar.Y / 65536) <= 209) && ((Conn.CompCar.Y / 65536) >= 41))
                            {
                                Conn.LocationBox = "^7J1 ^2Safe Zone ^380 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^1PIT", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Safe Zone";
                                Conn.LastSeen = "Safe Zone, SO4X";
                            }
                            //X200 Y-19 X244 Y45
                            else if (((Conn.CompCar.X / 65536) <= 244) && ((Conn.CompCar.X / 65536) >= 200) && ((Conn.CompCar.Y / 65536) <= 45) && ((Conn.CompCar.Y / 65536) >= -19))
                            {
                                Conn.LocationBox = "^7J1 ^2Safe Zone ^380 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^1PIT", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Safe Zone";
                                Conn.LastSeen = "Safe Zone, SO4X";
                            }
                            //X-348 Y361 X-404 Y497
                            else if (((Conn.CompCar.X / 65536) <= -348) && ((Conn.CompCar.X / 65536) >= -404) && ((Conn.CompCar.Y / 65536) <= 497) && ((Conn.CompCar.Y / 65536) >= 361))
                            {
                                Conn.LocationBox = "^3R3 ^7Under the Bridge ^380 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^1PIT", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Paying Zone";
                                Conn.LastSeen = "Paying Zone, SO4X";
                            }
                            //X-365 Y345 X-119 Y360
                            else if (((Conn.CompCar.X / 65536) <= -119) && ((Conn.CompCar.X / 65536) >= -365) && ((Conn.CompCar.Y / 65536) <= 360) && ((Conn.CompCar.Y / 65536) >= 345))
                            {
                                Conn.LocationBox = "^7J19 Highway ^3120 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0120", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Highway";
                                Conn.LastSeen = "^7Highway, SO4X";
                            }
                            //X-149 Y361 X-168 Y468
                            else if (((Conn.CompCar.X / 65536) <= -149) && ((Conn.CompCar.X / 65536) >= -168) && ((Conn.CompCar.Y / 65536) <= 468) && ((Conn.CompCar.Y / 65536) >= 361))
                            {
                                Conn.LocationBox = "^7J6 Ice Road ^390 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^090", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "^7Ice Road";
                                Conn.LastSeen = "^7Ice Road, SO4X";
                            }
                            //X-156 Y492 X-208 Y597
                            else if (((Conn.CompCar.X / 65536) <= -156) && ((Conn.CompCar.X / 65536) >= -208) && ((Conn.CompCar.Y / 65536) <= 597) && ((Conn.CompCar.Y / 65536) >= 492))
                            {
                                Conn.LocationBox = "^7J14 Dangerous Turns ^3100 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0100", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "^9Dangerous Turns";
                                Conn.LastSeen = "^9Dangerous Turns, SO4X";
                            }
                            //X-371 Y499 X-501 Y513
                            else if (((Conn.CompCar.X / 65536) <= -371) && ((Conn.CompCar.X / 65536) >= -501) && ((Conn.CompCar.Y / 65536) <= 513) && ((Conn.CompCar.Y / 65536) >= 499))
                            {
                                Conn.LocationBox = "^7J11 Old Mountain ^390 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^090", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "^7Old Mountain";
                                Conn.LastSeen = "^7Old Mountain, so4x";
                            }
                            else
                            {
                                Conn.LocationBox = "^7J18 Off Road ^350 km/h";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 50, 70, 22, 158, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 39, 58, 28, 164, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^050", Flags.ButtonStyles.ISB_C1, 5, 18, 46, 184, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Offroad";
                                Conn.LastSeen = "Offroad, Off Road";
                            }
                        }
                        #endregion

                        break;
                }

                #endregion

                #region ' Anti Hack Cheating System '
                bool DetectHacks = false;
                if (GameMode == 0)
                {
                    if (Conn.CurrentCar == "UF1")
                    {
                        if (kmh > 185)
                        {
                            DetectHacks = true;
                        }
                    }
                    if (Conn.CurrentCar == "XFG")
                    {
                        if (kmh > 210)
                        {
                            DetectHacks = true;
                        }
                    }
                    if (Conn.CurrentCar == "XRG")
                    {
                        if (kmh > 210)
                        {
                            DetectHacks = true;
                        }
                    }
                    if (Conn.CurrentCar == "LX4")
                    {
                        if (kmh > 222)
                        {
                            DetectHacks = true;
                        }
                    }
                    if (Conn.CurrentCar == "FBM")
                    {
                        if (kmh > 225)
                        {
                            DetectHacks = true;
                        }
                    }
                }
                if (DetectHacks == true)
                {
                    MsgAll("^1› " + Conn.PlayerName + " ^1: Speed Hack Detected");
                    MsgAll("^1› Current Car : " + Conn.CurrentCar + " Speed : " + kmh + "kmh / " + mph + "mph");
                    MsgPly("^9 You are banned for 12hours and remove your Speed Hack.", Conn.UniqueID);
                    BanID(Conn.Username, 0);
                }
                #endregion

                #region ' Busted/Lost/Ran Away/Released '

                if (Conn.Chasee != -1)
                {
                    byte ChaseeIndex = 0;
                    #region ' UniqueID Identifier '
                    for (byte o = 0; o < Connections.Count; o++)
                    {
                        if (Conn.Chasee == Connections[o].UniqueID)
                        {
                            ChaseeIndex = o;
                        }
                    }
                    #endregion
                    int BustedDist = ((int)Math.Sqrt(Math.Pow(ChaseCon.CompCar.X - Conn.CompCar.X, 2) + Math.Pow(ChaseCon.CompCar.Y - Conn.CompCar.Y, 2)) / 65536);

                    #region ' Busted '
                    if (BustedDist < 6 && ChaseCon.CompCar.Speed / 91 < 5)
                    {
                        if (Conn.Busted == false)
                        {
                            if (Conn.BustedTimer == 0)
                            {
                                Conn.BustedTimer = 1;
                            }
                            if (Conn.BustedTimer == 5)
                            {
                                #region ' Connection List '
                                /*foreach (clsConnection Con in Connections)
                            {
                                if (Con.Chasee == ChaseCon.UniqueID)
                                {
                                    if (Con.JoinedChase == true)
                                    {
                                        if (Con.Busted == false)
                                        {
                                            MsgPly("^3!!!^7Suspect is now stopped the Vehicle^3!!!", Con.UniqueID);
                                            MsgPly("^9 Hit the Busted button or type ^2!busted", Con.UniqueID);
                                            Con.Busted = true;
                                        }
                                    }
                                    if (Con.JoinedChase == false && Con.Busted == false)
                                    {
                                        Con.Busted = true;
                                    }
                                }
                            }*/
                                #endregion

                                #region ' Connection '

                                MsgPly("^3!!!^7Suspect is now stopped the Vehicle^3!!!", Conn.UniqueID);
                                MsgPly("^9 Hit the Busted button or type ^2!busted", Conn.UniqueID);
                                Conn.Busted = true;

                                #endregion
                            }
                        }
                    }
                    else
                    {
                        #region ' Get Away '
                        if (Conn.BustedTimer > 0)
                        {
                            if (Conn.Busted == true)
                            {
                                Conn.Busted = false;
                            }
                            Conn.BustedTimer = 0;
                        }
                        #endregion
                    }
                    #endregion

                    #region ' Suspect Lost '
                    if (BustedDist > 500)
                    {
                        if (Conn.InChaseProgress == true)
                        {
                            if (Connections[ChaseeIndex].CopInChase > 1)
                            {
                                if (Conn.JoinedChase == true)
                                {
                                    Conn.JoinedChase = false;
                                }

                                Conn.AutoBumpTimer = 0;
                                Conn.BumpButton = 0;
                                Conn.BustedTimer = 0;
                                Conn.Busted = false;
                                Conn.ChaseCondition = 0;
                                Conn.Chasee = -1;

                                #region ' Connection List '
                                foreach (clsConnection Con in Connections)
                                {
                                    if (Con.Chasee == ChaseCon.UniqueID)
                                    {
                                        if (ChaseCon.CopInChase == 1)
                                        {
                                            if (Con.JoinedChase == true)
                                            {
                                                Con.JoinedChase = false;
                                            }
                                        }
                                    }
                                }
                                #endregion

                                Connections[ChaseeIndex].CopInChase -= 1;
                                MsgAll("^9 " + Conn.PlayerName + " lost sighting " + Connections[ChaseeIndex].PlayerName + "!");
                                MsgAll("   ^7Total Cops In Chase: " + Connections[ChaseeIndex].CopInChase);
                            }

                            else if (Connections[ChaseeIndex].CopInChase == 1)
                            {
                                MsgAll("^9 " + Conn.PlayerName + " lost " + ChaseCon.PlayerName + "!");
                                MsgAll("   ^7Suspect Runs away from being chased!");

                                AddChaseLimit -= 1;
                                Conn.AutoBumpTimer = 0;
                                Conn.BumpButton = 0;
                                Conn.BustedTimer = 0;
                                Conn.Busted = false;
                                Connections[ChaseeIndex].CopInChase = 0;
                                Connections[ChaseeIndex].IsSuspect = false;
                                Connections[ChaseeIndex].ChaseCondition = 0;
                                Conn.ChaseCondition = 0;
                                Conn.Chasee = -1;
                                CopSirenShutOff();
                            }
                            Conn.InChaseProgress = false;
                        }
                    }
                    #endregion
                }

                #region ' Not paying Fines! '
                if (Conn.IsBeingBusted == true)
                {
                    if (kmh > 40)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " was fined ^1$5000");
                        MsgAll("  ^7For running away on ticket whilst being busted!");
                        Conn.Cash -= 5000;

                        #region ' In Connection List '

                        foreach (clsConnection i in Connections)
                        {
                            if (i.Chasee == Conn.UniqueID)
                            {
                                if (i.InFineMenu == true)
                                {
                                    #region ' Close Region LOL '
                                    DeleteBTN(30, i.UniqueID);
                                    DeleteBTN(31, i.UniqueID);
                                    DeleteBTN(32, i.UniqueID);
                                    DeleteBTN(33, i.UniqueID);
                                    DeleteBTN(34, i.UniqueID);
                                    DeleteBTN(35, i.UniqueID);
                                    DeleteBTN(36, i.UniqueID);
                                    DeleteBTN(37, i.UniqueID);
                                    DeleteBTN(38, i.UniqueID);
                                    DeleteBTN(39, i.UniqueID);
                                    DeleteBTN(40, i.UniqueID);
                                    #endregion

                                    i.InFineMenu = false;
                                }
                                if (i.IsOfficer == true)
                                {
                                    MsgAll("^9 " + i.PlayerName + " was rewarded for ^2$" + (Convert.ToInt16(5000 * 0.4)));
                                    i.Cash += (Convert.ToInt16(5000 * 0.4));
                                }
                                if (i.IsCadet == true)
                                {
                                    MsgAll("^9 " + i.PlayerName + " was rewarded for ^2$" + (Convert.ToInt16(5000 * 0.4)));
                                    i.Cash += (Convert.ToInt16(5000 * 0.2));
                                }
                                if (i.JoinedChase == true)
                                {
                                    i.JoinedChase = false;
                                }
                                i.TicketAmount = 0;
                                i.TicketAmountSet = false;
                                i.TicketReason = "";
                                i.TicketReasonSet = false;
                                i.Busted = false;
                                i.Chasee = -1;
                                i.ChaseCondition = 0;
                            }
                        }

                        #endregion

                        #region ' Close Region LOL '
                        DeleteBTN(30, Conn.UniqueID);
                        DeleteBTN(31, Conn.UniqueID);
                        DeleteBTN(32, Conn.UniqueID);
                        DeleteBTN(33, Conn.UniqueID);
                        DeleteBTN(34, Conn.UniqueID);
                        DeleteBTN(35, Conn.UniqueID);
                        DeleteBTN(36, Conn.UniqueID);
                        DeleteBTN(37, Conn.UniqueID);
                        DeleteBTN(38, Conn.UniqueID);
                        DeleteBTN(39, Conn.UniqueID);
                        DeleteBTN(40, Conn.UniqueID);
                        #endregion

                        Conn.ChaseCondition = 0;
                        Conn.AcceptTicket = 0;
                        Conn.TicketRefuse = 0;
                        Conn.CopInChase = 0;
                        Conn.IsBeingBusted = false;
                    }
                }
                #endregion

                #region ' Released by the cop! '
                if (Conn.InFineMenu == true)
                {
                    if (kmh > 40)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " released " + ChaseCon.PlayerName + "!");

                        #region ' Chasee Connection '
                        foreach (clsConnection i in Connections)
                        {
                            if (i.UniqueID == ChaseCon.UniqueID)
                            {
                                if (i.IsBeingBusted == true)
                                {
                                    if (i.AcceptTicket == 1)
                                    {
                                        #region ' Close Region LOL '
                                        DeleteBTN(30, i.UniqueID);
                                        DeleteBTN(31, i.UniqueID);
                                        DeleteBTN(32, i.UniqueID);
                                        DeleteBTN(33, i.UniqueID);
                                        DeleteBTN(34, i.UniqueID);
                                        DeleteBTN(35, i.UniqueID);
                                        DeleteBTN(36, i.UniqueID);
                                        DeleteBTN(37, i.UniqueID);
                                        DeleteBTN(38, i.UniqueID);
                                        DeleteBTN(39, i.UniqueID);
                                        DeleteBTN(40, i.UniqueID);
                                        #endregion
                                        i.AcceptTicket = 0;
                                    }
                                    i.ChaseCondition = 0;
                                    i.AcceptTicket = 0;
                                    i.TicketRefuse = 0;
                                    i.CopInChase = 0;
                                    i.IsBeingBusted = false;
                                }
                            }

                            if (i.Chasee == ChaseCon.UniqueID)
                            {
                                if (i.JoinedChase == true)
                                {
                                    i.JoinedChase = false;
                                }
                                i.TicketAmount = 0;
                                i.TicketAmountSet = false;
                                i.TicketReason = "";
                                i.TicketReasonSet = false;
                                i.Busted = false;
                                i.Chasee = -1;
                                i.ChaseCondition = 0;
                            }
                        }
                        #endregion

                        #region ' Close Region LOL '
                        DeleteBTN(30, Conn.UniqueID);
                        DeleteBTN(31, Conn.UniqueID);
                        DeleteBTN(32, Conn.UniqueID);
                        DeleteBTN(33, Conn.UniqueID);
                        DeleteBTN(34, Conn.UniqueID);
                        DeleteBTN(35, Conn.UniqueID);
                        DeleteBTN(36, Conn.UniqueID);
                        DeleteBTN(37, Conn.UniqueID);
                        DeleteBTN(38, Conn.UniqueID);
                        DeleteBTN(39, Conn.UniqueID);
                        DeleteBTN(40, Conn.UniqueID);
                        #endregion

                        if (Conn.InFineMenu == true)
                        {
                            Conn.InFineMenu = false;
                        }

                        Conn.Busted = false;
                    }
                }
                #endregion

                #endregion

                #region ' Lost Towing '

                if (Conn.Towee != -1)
                {
                    byte TowIndex = 0;
                    #region ' UniqueID Identifier '
                    for (byte o = 0; o < Connections.Count; o++)
                    {
                        if (Conn.Towee == Connections[o].UniqueID)
                        {
                            TowIndex = o;
                        }
                    }
                    #endregion

                    int TowDist = ((int)Math.Sqrt(Math.Pow(Connections[TowIndex].CompCar.X - Conn.CompCar.X, 2) + Math.Pow(Connections[TowIndex].CompCar.Y - Conn.CompCar.Y, 2)) / 65536);
                    if (TowDist > 250)
                    {
                        if (Conn.InTowProgress == true)
                        {
                            if (Connections[TowIndex].IsBeingTowed == true)
                            {
                                MsgAll("^9 " + Conn.PlayerName + " stopped towing " + Connections[TowIndex].PlayerName + "!");
                                Connections[TowIndex].IsBeingTowed = false;
                            }
                            Conn.Towee = -1;
                            Conn.InTowProgress = false;
                            CautionSirenShutOff();
                        }
                    }
                }

                #endregion

                #region ' Speed Trap '

                if (Conn.TrapSpeed > 0)
                {
                    int TrapDist = 0;
                    byte Speeders = 0;
                    if (Conn.TrapSetted == true)
                    {
                        TrapDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (Conn.TrapX * 196608), 2) + Math.Pow(Conn.CompCar.Y - (Conn.TrapY * 196608), 2)) / 65536);
                        if (TrapDist > 10)
                        {
                            MsgPly("^9 Speed Trap Removed", Conn.UniqueID);
                            Conn.TrapY = 0;
                            Conn.TrapX = 0;
                            Conn.TrapSpeed = 0;
                            Conn.TrapSetted = false;
                        }
                    }
                    for (byte i = 0; i < Connections.Count; i++)
                    {
                        if (Connections[i].PlayerID != 0)
                        {
                            Speeders = i;
                        }
                        var TrapVar = Connections[Speeders];
                        if (TrapVar.IsOfficer == false && TrapVar.IsCadet == false && TrapVar.IsSuspect == false && TrapVar.CompCar.Speed / 91 > Conn.TrapSpeed && TrapVar.InTrap == false)
                        {
                            TrapDist = ((int)Math.Sqrt(Math.Pow(TrapVar.CompCar.X - (Conn.TrapX * 196608), 2) + Math.Pow(TrapVar.CompCar.Y - (Conn.TrapY * 196608), 2)) / 65536);
                            if (TrapDist < 30 && Conn.TrapSetted == true)
                            {
                                int ExcessSpeed = (int)(TrapVar.CompCar.Speed / 91 - Conn.TrapSpeed / 2);
                                int SpeedFine = ExcessSpeed * 2;

                                MsgAll("^9 Over Speeding User Detected!");
                                MsgAll("^9 Speeding: " + TrapVar.PlayerName + " (" + TrapVar.Username + ")");
                                MsgAll("^9 Clocked at ^1" + TrapVar.CompCar.Speed / 91 + " kmh ^7/ ^1" + TrapVar.CompCar.Speed / 146 + " mph");
                                MsgAll("^9 " + TrapVar.PlayerName + " was fined ^1$" + SpeedFine);
                                MsgAll("^9 Speed Trapper: " + Conn.PlayerName + " (" + Conn.Username + ")");
                                MsgAll("^9 " + Conn.PlayerName + " was rewarded ^2$" + (Convert.ToInt16(SpeedFine * 0.4)));
                                Conn.Cash += (Convert.ToInt16(SpeedFine * 0.4));
                                TrapVar.Cash -= SpeedFine;
                                TrapVar.InTrap = true;

                            }
                            else
                            {
                                if (TrapVar.InTrap == true)
                                {
                                    TrapVar.InTrap = false;
                                }
                            }
                        }
                    }
                }


                #endregion
            }
            catch { }
        }
    }
}