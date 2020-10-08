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
        #region ' Event Loaders '

        // Form1 Loader
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                #region ' Chat Command Loader '
                foreach (MethodInfo mi in typeof(Form1).GetMethods())
                {
                    foreach (object o in mi.GetCustomAttributes(false))
                    {
                        if (o.GetType() == typeof(CommandAttribute))
                        {
                            CommandAttribute ca = (CommandAttribute)o;
                            CommandList com = new CommandList();
                            com.CommandArg = ca;
                            com.MethodInf = mi;
                            Commands.Add(com);
                        }
                    }
                }
                #endregion

                if (System.IO.Directory.Exists(Database) == false)
                {
                    System.IO.Directory.CreateDirectory(Database);
                }

                #region ' Check User '

                System.Timers.Timer CheckUser = new System.Timers.Timer(5000);
                CheckUser.Elapsed += new System.Timers.ElapsedEventHandler(CheckUser_Elapsed);
                CheckUser.Enabled = true;

                #endregion

                #region ' Second timers '

                System.Timers.Timer SecondTimers = new System.Timers.Timer(1000);
                SecondTimers.Elapsed += new System.Timers.ElapsedEventHandler(SecondTimers_Elapsed);
                SecondTimers.Enabled = true;

                #endregion

                #region ' OnScreen Effects '

                System.Timers.Timer OnScreen = new System.Timers.Timer(500);
                OnScreen.Elapsed += new System.Timers.ElapsedEventHandler(OnScreen_Elapsed);
                OnScreen.Enabled = true;
                #endregion

                #region ' Save Timer '

                // 2.5 Minutes
                System.Timers.Timer BackUp_Users = new System.Timers.Timer(150000);
                BackUp_Users.Elapsed += new System.Timers.ElapsedEventHandler(BackUp_Users_Elapsed);
                BackUp_Users.Enabled = true;

                #endregion

                #region ' 1 Minute Interval '

                System.Timers.Timer MinuteTimer = new System.Timers.Timer(60000);
                MinuteTimer.Elapsed += new System.Timers.ElapsedEventHandler(MinuteTimer_Elapsed);
                MinuteTimer.Enabled = true;

                #endregion

                #region ' Siren '

                System.Timers.Timer SystemSiren = new System.Timers.Timer(200);
                SystemSiren.Elapsed += new System.Timers.ElapsedEventHandler(SystemSiren_Elapsed);
                SystemSiren.Enabled = true;

                #endregion
            }
            catch { }
        }

        // System Siren
        private void SystemSiren_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                foreach (clsConnection Conn in Connections)
                {
                    if ((Conn.PlayerName == HostName && Conn.UniqueID == 0) == false)
                    {
                        #region ' Show Siren '
                        if (Conn.InGame == 1)
                        {
                            #region ' Siren Check '
                            if (Conn.InChaseProgress != false)
                            {
                                byte SirenIndex = 0;

                                #region ' Remote Siren PLID '
                                for (byte o = 0; o < Connections.Count; o++)
                                {
                                    if (Connections[o].PlayerID != 0)
                                    {
                                        SirenIndex = o;
                                    }

                                    #region ' Get Siren '
                                    var ChaseCon = Connections[SirenIndex];
                                    ChaseCon.SirenDistance = ((int)Math.Sqrt(Math.Pow(ChaseCon.CompCar.X - (Conn.CompCar.X), 2) + Math.Pow(ChaseCon.CompCar.Y - (Conn.CompCar.Y), 2)) / 65536);
                                    if (ChaseCon.InChaseProgress == false)
                                    {
                                        if (ChaseCon.SirenDistance < 250)
                                        {
                                            ChaseCon.SirenShowned = true;
                                            #region ' Siren Index '
                                            if (ChaseCon.CopSiren == 0)
                                            {
                                                ChaseCon.CopSiren = 1;
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            if (ChaseCon.SirenShowned == true)
                                            {
                                                if (ChaseCon.CopSiren != 0)
                                                {
                                                    ChaseCon.CopSiren = 3;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ChaseCon.SirenShowned == true)
                                        {
                                            if (ChaseCon.CopSiren != 0)
                                            {
                                                ChaseCon.CopSiren = 3;
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            #endregion

                            #region ' Show Siren '

                            if (Conn.CopSiren != 0 && Conn.SirenShowned != false)
                            {
                                if (Conn.CopSiren == 1)
                                {
                                    Conn.CopSiren = 10;
                                }
                                else if (Conn.CopSiren > 9)
                                {
                                    if (Conn.CopSiren > 14)
                                    {
                                        Conn.CopSiren = 10;
                                    }

                                    if (OnScreen == 0)
                                    {
                                        //InSim.Send_BTN_CreateButton("^4" + "((((".Insert((Conn.CopSiren % 10), + "^4^J£ ^7SIREN ^1^J£" + "^4" + "))))".Insert(4 - (Conn.CopSiren % 10), "^1)^4"), Flags.ButtonStyles.ISB_C1, 15, 74, 60, 63, 23, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^4" + "((((".Insert((Conn.CopSiren % 10), "^1(^4") + "^4^J£ ^7" + "SIREN ^1^J£" + "^4" + "))))".Insert(4 - (Conn.CopSiren % 10), "^1)^4"), Flags.ButtonStyles.ISB_C1, 15, 74, 60, 63, 23, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        //InSim.Send_BTN_CreateButton("^4^J£ ^7SIREN ^1^J£", Flags.ButtonStyles.ISB_C1, 15, 74, 60, 63, 23, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^4" + "((((".Insert((Conn.CopSiren % 10), "^1(^4") + "^1^J£ ^7" + "SIREN ^4^J£" + "^4" + "))))".Insert(4 - (Conn.CopSiren % 10), "^1)^4"), Flags.ButtonStyles.ISB_C1, 15, 74, 60, 63, 23, Conn.UniqueID, 2, false);
                                    }

                                    InSim.Send_BTN_CreateButton("^1" + Conn.SirenDistance + " meters", Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 5, 74, 73, 93, 24, Conn.UniqueID, 2, false);
                                    Conn.CopSiren++;
                                }
                                else if (Conn.CopSiren == 3)
                                {
                                    if (Conn.SirenShowned == true)
                                    {
                                        DeleteBTN(23, Conn.UniqueID);
                                        DeleteBTN(24, Conn.UniqueID);
                                        Conn.CopSiren = 0;
                                        Conn.SirenShowned = false;
                                    }
                                }
                            }
                            #endregion

                            #region ' Caution Siren Check '
                            if (Conn.InTowProgress != false)
                            {
                                byte SirenIndex = 0;

                                #region ' Remote Siren PLID '
                                for (byte o = 0; o < Connections.Count; o++)
                                {
                                    if (Connections[o].PlayerID != 0)
                                    {
                                        SirenIndex = o;
                                    }

                                    #region ' Get Siren '
                                    var ChaseCon = Connections[SirenIndex];
                                    ChaseCon.SirenDistance = ((int)Math.Sqrt(Math.Pow(ChaseCon.CompCar.X - (Conn.CompCar.X), 2) + Math.Pow(ChaseCon.CompCar.Y - (Conn.CompCar.Y), 2)) / 65536);
                                    if (ChaseCon.InTowProgress == false)
                                    {
                                        if (ChaseCon.SirenDistance < 250)
                                        {
                                            ChaseCon.SirenShowned = true;
                                            #region ' Siren Index '
                                            if (ChaseCon.TowCautionSiren == 0)
                                            {
                                                ChaseCon.TowCautionSiren = 1;
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            if (ChaseCon.SirenShowned == true)
                                            {
                                                if (ChaseCon.TowCautionSiren != 0)
                                                {
                                                    ChaseCon.TowCautionSiren = 3;
                                                }
                                                ChaseCon.SirenShowned = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ChaseCon.SirenShowned == true)
                                        {
                                            if (ChaseCon.TowCautionSiren != 0)
                                            {
                                                ChaseCon.TowCautionSiren = 3;
                                            }
                                            ChaseCon.SirenShowned = false;
                                        }
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            #endregion

                            #region ' Show Siren '

                            if (Conn.TowCautionSiren != 0 && Conn.SirenShowned != false)
                            {
                                if (Conn.TowCautionSiren == 1)
                                {
                                    Conn.TowCautionSiren = 10;
                                }
                                else if (Conn.TowCautionSiren > 9)
                                {
                                    if (Conn.TowCautionSiren > 14)
                                    {
                                        Conn.TowCautionSiren = 10;
                                    }

                                    if (OnScreen == 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^3" + "((((".Insert((Conn.TowCautionSiren % 10), "^1(^3") + "^1(^3!^1) ^7" + "CAUTION ^3(^1!^3)" + "^3" + "))))".Insert(4 - (Conn.TowCautionSiren % 10), "^1)^3"), Flags.ButtonStyles.ISB_C1, 15, 74, 60, 63, 23, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^3" + "((((".Insert((Conn.TowCautionSiren % 10), "^1(^3") + "^3(^1!^3) ^7" + "CAUTION ^1(^3!^1)" + "^3" + "))))".Insert(4 - (Conn.TowCautionSiren % 10), "^1)^3"), Flags.ButtonStyles.ISB_C1, 15, 74, 60, 63, 23, Conn.UniqueID, 2, false);
                                    }

                                    InSim.Send_BTN_CreateButton("^1" + Conn.SirenDistance + " meters", Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 5, 74, 73, 93, 24, Conn.UniqueID, 2, false);
                                    Conn.TowCautionSiren++;
                                }
                                else if (Conn.TowCautionSiren == 3)
                                {
                                    if (Conn.SirenShowned == true)
                                    {
                                        DeleteBTN(23, Conn.UniqueID);
                                        DeleteBTN(24, Conn.UniqueID);
                                        Conn.TowCautionSiren = 0;
                                        Conn.SirenShowned = false;
                                    }
                                }
                            }
                            #endregion
                        }

                        #region ' Spectators '
                        else if (Conn.InGame == 0)
                        {
                            if (Conn.CopSiren != 0)
                            {
                                Conn.CopSiren = 3;
                            }
                            else if (Conn.CopSiren == 3)
                            {
                                DeleteBTN(23, Conn.UniqueID);
                                DeleteBTN(24, Conn.UniqueID);
                                Conn.CopSiren = 0;
                            }
                            DeleteBTN(23, Conn.UniqueID);
                            DeleteBTN(24, Conn.UniqueID);
                        }
                        #endregion

                        #endregion

                    }
                }
            }
            catch { }
        }

        // Minute Timers
        private void MinuteTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                #region ' Connection '

                foreach (clsConnection Conn in Connections)
                {
                    if ((Conn.PlayerName == HostName && Conn.UniqueID == 0) == false)
                    {
                        #region ' Last Raffle Timer '

                        if (Conn.LastRaffle == 0 == false)
                        {
                            if (Conn.LastRaffle > 0)
                            {
                                Conn.LastRaffle -= 1;
                            }
                            #region ' Display Countdown '
                            if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true && Conn.InStore == true)
                            {
                                if (Conn.LastRaffle > 120)
                                {
                                    InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1Three (3)hours ^7to Rejoin the Raffle!", Flags.ButtonStyles.ISB_LEFT, 4, 130, 73, 54, 116, Conn.UniqueID, 2, false);
                                }
                                else if (Conn.LastRaffle > 60)
                                {
                                    InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1Two (2)hours ^7to Rejoin the Raffle!", Flags.ButtonStyles.ISB_LEFT, 4, 130, 73, 54, 116, Conn.UniqueID, 2, false);
                                }
                                else if (Conn.LastRaffle > 0)
                                {
                                    if (Conn.LastRaffle > 1)
                                    {
                                        InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1" + Conn.LastRaffle + " minutes ^7to Rejoin the Raffle!", Flags.ButtonStyles.ISB_LEFT, 4, 130, 73, 54, 116, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1" + Conn.LastRaffle + " minute ^7to Rejoin the Raffle!", Flags.ButtonStyles.ISB_LEFT, 4, 130, 73, 54, 116, Conn.UniqueID, 2, false);
                                    }
                                }
                                else
                                {
                                    if (Conn.TotalSale > 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Total Item bought: " + Conn.TotalSale, Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Raffle!", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 100, 120, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^7You must buy a Item before you can Join the Raffle!", Flags.ButtonStyles.ISB_LEFT, 4, 130, 73, 54, 116, Conn.UniqueID, 2, false);
                                    }
                                }
                            }
                            #endregion
                        }

                        #endregion

                        #region ' Last Lotto Timer '

                        if (Conn.LastLotto == 0 == false)
                        {
                            if (Conn.LastLotto > 0)
                            {
                                Conn.LastLotto -= 1;
                            }
                            #region ' Display Countdown '
                            if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true && Conn.InSchool == true)
                            {
                                if (Conn.LastLotto > 120)
                                {
                                    InSim.Send_BTN_CreateButton("^7You have to wait ^1Three (3) hours ^7to rejoin the Lotto", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);
                                }
                                else if (Conn.LastLotto > 60)
                                {
                                    InSim.Send_BTN_CreateButton("^7You have to wait ^1Two (2) hours ^7to rejoin the Lotto", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);
                                }
                                else if (Conn.LastLotto > 0)
                                {
                                    if (Conn.LastLotto > 1)
                                    {
                                        InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1" + Conn.LastRaffle + " minutes ^7to rejoin the Lotto!", Flags.ButtonStyles.ISB_LEFT, 4, 130, 73, 54, 116, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1" + Conn.LastRaffle + " minute ^7to rejoin the Lotto!", Flags.ButtonStyles.ISB_LEFT, 4, 130, 73, 54, 116, Conn.UniqueID, 2, false);
                                    }
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^2Try ^7Lotto Costs: ^1$100", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Lotto", "Pick your number 1 - 10", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 100, 2, 120, Conn.UniqueID, 2, false);
                                }
                            }
                            #endregion
                        }

                        #endregion
                    }
                }

                #endregion
            }
            catch { }
        }

        // Save Users 2.5 Minutes
        private void BackUp_Users_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                #region ' Save Users '
                if (Connections.Count > 1)
                {
                    foreach (clsConnection C in Connections)
                    {
                        if (C.FailCon == 0)
                        {
                            FileInfo.SaveUser(C);
                        }
                    }
                    //MsgAll("^3Stat Auto-Saved.");
                }
                #endregion
            }
            catch
            {
                #region ' Save Failed '
                if (Connections.Count > 1)
                {
                    MsgAll("^1Stat Auto-Save Failed.");
                }
                #endregion
            }
        }

        // OnScreen Effects 500ms (.50 milliseconds)
        private void OnScreen_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                foreach (clsConnection Conn in Connections)
                {
                    if ((Conn.PlayerName == HostName && Conn.UniqueID == 0) == false)
                    {
                        #region ' OnScreen Effects '
                        if (Conn.OnScreenExit > 0)
                        {
                            if (OnScreen == 0)
                            {
                                InSim.Send_BTN_CreateButton("^7›› ^1PIT EXIT ^7››", Flags.ButtonStyles.ISB_DARK, 20, 50, 40, 77, 10, Conn.UniqueID, 2, false);
                            }
                            else
                            {
                                InSim.Send_BTN_CreateButton("^1›› ^7PIT EXIT ^1››", Flags.ButtonStyles.ISB_DARK, 20, 50, 40, 77, 10, Conn.UniqueID, 2, false);
                            }
                        }
                        #endregion

                        #region ' Clear OnScreen '

                        if (Conn.OnScreenExit == 0 == false)
                        {
                            if (Conn.OnScreenExit > 0)
                            {
                                Conn.OnScreenExit -= 1;
                            }
                            if (Conn.OnScreenExit == 0)
                            {
                                DeleteBTN(10, Conn.UniqueID);
                                Conn.LeavesPitLane = 0;
                            }
                        }

                        #endregion
                    }
                }

                #region ' Effects '
                if (OnScreen == 0)
                {
                    OnScreen = 1;
                }
                else
                {
                    OnScreen = 0;
                }
                #endregion
            }
            catch { }
        }

        // Second Timers & Tick Event 1000ms (1 Second)
        private void SecondTimers_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                foreach (clsConnection Conn in Connections)
                {
                    if ((Conn.PlayerName == HostName && Conn.UniqueID == 0) == false)
                    {
                        #region ' CompCar Variables '
                        var kmh = Conn.CompCar.Speed / 91;
                        #endregion

                        #region ' Speed Limit and Location '
                            InSim.Send_BTN_CreateButton(Conn.LocationBox, Flags.ButtonStyles.ISB_DARK, 4, 31, 8, 168, 9, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("Trip: " + Conn.TripMeter.ToString("F2") + " ^J‡q", Flags.ButtonStyles.ISB_DARK, 4, 16, 12, 183, 8, Conn.UniqueID, 2, false);

                        #endregion

                        #region ' Cop Panel '

                        if (Conn.IsOfficer == true || Conn.IsCadet == true)
                        {
                            if (Conn.InGame == 1)
                            {
                                InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 30, 27, 121, 170, 15, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7Cop Panel", Flags.ButtonStyles.ISB_LIGHT, 4, 25, 123, 171, 16, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7Cop Version: v1.0B", Flags.ButtonStyles.ISB_LIGHT, 4, 25, 145, 171, 22, Conn.UniqueID, 2, false);

                                clsConnection ChaseCon = Connections[GetConnIdx(Conn.Chasee)];

                                if (Conn.InChaseProgress == false)
                                {
                                    bool DetectSpeeders = false;
                                    byte Speeders = 0;

                                    #region ' Remote Speeder PLID '
                                    for (byte o = 0; o < Connections.Count; o++)
                                    {
                                        if (Connections[o].PlayerID != 0)
                                        {
                                            Speeders = o;
                                        }

                                        #region ' Detected '
                                        if (Connections[Speeders].IsOfficer == false && Connections[Speeders].IsCadet == false)
                                        {
                                            int SpeederDist = ((int)Math.Sqrt(Math.Pow(Connections[Speeders].CompCar.X - Conn.CompCar.X, 2) + Math.Pow(Connections[Speeders].CompCar.Y - Conn.CompCar.Y, 2)) / 65536);
                                            if (SpeederDist < 50)
                                            {
                                                if (DetectSpeeders == false)
                                                {
                                                    #region ' Panel '
                                                    InSim.Send_BTN_CreateButton("^7Name: " + Connections[Speeders].PlayerName, Flags.ButtonStyles.ISB_DARK, 4, 25, 128, 171, 17, Conn.UniqueID, 2, false);
                                                    if (Connections[Speeders].IsSpeeder == 0)
                                                    {
                                                        if (Conn.KMHorMPH == 0)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^7Car: (^3" + Connections[Speeders].CurrentCar + "^7) Speed: ^2" + Connections[Speeders].CompCar.Speed / 91 + " kmh", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                                        }
                                                        else if (Conn.KMHorMPH == 1)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^7Car: (^3" + Connections[Speeders].CurrentCar + "^7) Speed: ^2" + Connections[Speeders].CompCar.Speed / 146 + " mph", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                                        }
                                                    }
                                                    else if (Connections[Speeders].IsSpeeder == 1)
                                                    {
                                                        if (Conn.KMHorMPH == 0)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^7Car: (^3" + Connections[Speeders].CurrentCar + "^7) Speed: ^1" + Connections[Speeders].CompCar.Speed / 91 + " kmh", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                                        }
                                                        else if (Conn.KMHorMPH == 1)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^7Car: (^3" + Connections[Speeders].CurrentCar + "^7) Speed: ^1" + Connections[Speeders].CompCar.Speed / 146 + " mph", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                                        }
                                                    }
                                                    InSim.Send_BTN_CreateButton("^7Distance: ^3" + SpeederDist + " ^7meters", Flags.ButtonStyles.ISB_DARK, 4, 25, 136, 171, 19, Conn.UniqueID, 2, false);
                                                    #endregion

                                                    #region ' Speeder Clock '

                                                    if (Connections[Speeders].IsSpeeder == 1)
                                                    {
                                                        if (kmh > 3)
                                                        {
                                                            if (Conn.SpeederClocked == false)
                                                            {
                                                                MsgAll("^9 " + Conn.PlayerName + " clocked " + Connections[Speeders].PlayerName);
                                                                if (Conn.KMHorMPH == 0)
                                                                {
                                                                    MsgAll("^9 Car: (^3" + Connections[Speeders].CurrentCar + "^7) Speed: ^1" + Connections[Speeders].CompCar.Speed / 91 + " kmh");
                                                                }
                                                                else if (Conn.KMHorMPH == 1)
                                                                {
                                                                    MsgAll("^9 Car: (^3" + Connections[Speeders].CurrentCar + "^7) Speed: ^1" + Connections[Speeders].CompCar.Speed / 146 + " mph");
                                                                }
                                                                Conn.SpeederClocked = true;
                                                            }
                                                        }
                                                    }
                                                    else if (Connections[Speeders].IsSpeeder == 0)
                                                    {
                                                        if (Conn.SpeederClocked == true)
                                                        {
                                                            Conn.SpeederClocked = false;
                                                        }
                                                    }

                                                    #endregion

                                                    DetectSpeeders = true;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region ' Undetected '
                                    if (DetectSpeeders == false)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Name: None", Flags.ButtonStyles.ISB_DARK, 4, 25, 128, 171, 17, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7Car: None Speed: None", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7Distance: None", Flags.ButtonStyles.ISB_DARK, 4, 25, 136, 171, 19, Conn.UniqueID, 2, false);

                                        if (Conn.SpeederClocked == true)
                                        {
                                            Conn.SpeederClocked = false;
                                        }
                                    }
                                    #endregion

                                    #region ' Enable Click '
                                    if (Conn.CopPanel == 1)
                                    {
                                        if (Conn.Busted == false)
                                        {
                                            InSim.Send_BTN_CreateButton("^7ENGAGE", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                            #region ' Officer/Cadet Speed Trap Option '
                                            if (Conn.IsOfficer == true)
                                            {
                                                if (Conn.TrapSetted == false)
                                                {
                                                    if (kmh < 2)
                                                    {
                                                        InSim.Send_BTN_CreateButton("^7SPEED TRAP", "Set Speed Trap.", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 12, 140, 184, 3, 21, Conn.UniqueID, 2, false);
                                                    }
                                                    else
                                                    {
                                                        InSim.Send_BTN_CreateButton("^8SPEED TRAP", "Set Speed Trap.", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 3, 21, Conn.UniqueID, 2, false);
                                                    }
                                                }
                                                else
                                                {
                                                    InSim.Send_BTN_CreateButton("^7REMOVE TRAP", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 12, 140, 184, 21, Conn.UniqueID, 2, false);
                                                }
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^7", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 21, Conn.UniqueID, 2, false);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            InSim.Send_BTN_CreateButton("^8ENGAGE", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                            #region ' Officer/Cadet Speed Trap Option '
                                            if (Conn.IsOfficer == true)
                                            {
                                                if (Conn.TrapSetted == false)
                                                {

                                                    if (kmh < 2)
                                                    {
                                                        InSim.Send_BTN_CreateButton("^8SPEED TRAP", "Set Speed Trap.", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 3, 21, Conn.UniqueID, 2, false);
                                                    }
                                                    else
                                                    {
                                                        InSim.Send_BTN_CreateButton("^8SPEED TRAP", "Set Speed Trap.", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 3, 21, Conn.UniqueID, 2, false);
                                                    }
                                                }
                                                else
                                                {
                                                    InSim.Send_BTN_CreateButton("^8REMOVE TRAP", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 21, Conn.UniqueID, 2, false);
                                                }
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^8", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 21, Conn.UniqueID, 2, false);
                                            }
                                            #endregion
                                        }
                                    }
                                    else if (Conn.CopPanel == 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^8ENGAGE", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);

                                        #region ' Officer/Cadet Speed Trap Option '
                                        if (Conn.IsOfficer == true)
                                        {
                                            if (Conn.TrapSetted == false)
                                            {
                                                if (kmh < 2)
                                                {
                                                    InSim.Send_BTN_CreateButton("^8SPEED TRAP", "Set Speed Trap.", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 3, 21, Conn.UniqueID, 2, false);
                                                }
                                                else
                                                {
                                                    InSim.Send_BTN_CreateButton("^8SPEED TRAP", "Set Speed Trap.", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 3, 21, Conn.UniqueID, 2, false);
                                                }
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^8REMOVE TRAP", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 21, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_BTN_CreateButton("^8", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 21, Conn.UniqueID, 2, false);
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                else
                                {
                                    int ChaseDist = ((int)Math.Sqrt(Math.Pow(ChaseCon.CompCar.X - Conn.CompCar.X, 2) + Math.Pow(ChaseCon.CompCar.Y - Conn.CompCar.Y, 2)) / 65536);

                                    #region ' Chase Condition '
                                    if (Conn.ChaseCondition == 1)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Condition: ^4^S¡ï" + "^7^J™" + "^7^J™" + "^7^J™" + "^7^J™", Flags.ButtonStyles.ISB_DARK, 4, 25, 128, 171, 17, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.ChaseCondition == 2)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Condition: ^4^S¡ï" + "^4^S¡ï" + "^7^J™" + "^7^J™" + "^7^J™", Flags.ButtonStyles.ISB_DARK, 4, 25, 128, 171, 17, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.ChaseCondition == 3)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Condition: ^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï" + "^7^J™" + "^7^J™", Flags.ButtonStyles.ISB_DARK, 4, 25, 128, 171, 17, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.ChaseCondition == 4)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Condition: ^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï" + "^7^J™", Flags.ButtonStyles.ISB_DARK, 4, 25, 128, 171, 17, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.ChaseCondition == 5)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Condition: ^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï", Flags.ButtonStyles.ISB_DARK, 4, 25, 128, 171, 17, Conn.UniqueID, 2, false);
                                    }
                                    #endregion

                                    #region ' Speed Check '
                                    if (ChaseCon.IsSpeeder == 0)
                                    {
                                        if (Conn.KMHorMPH == 0)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Car: (^3" + ChaseCon.CurrentCar + "^7) Speed: ^2" + ChaseCon.CompCar.Speed / 91 + " kmh", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                        }
                                        else if (Conn.KMHorMPH == 1)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Car: (^3" + ChaseCon.CurrentCar + "^7) Speed: ^2" + ChaseCon.CompCar.Speed / 146 + " mph", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                        }
                                    }
                                    else if (ChaseCon.IsSpeeder == 1)
                                    {
                                        if (Conn.KMHorMPH == 0)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Car: (^3" + ChaseCon.CurrentCar + "^7) Speed: ^1" + ChaseCon.CompCar.Speed / 91 + " kmh", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                        }
                                        else if (Conn.KMHorMPH == 1)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Car: (^3" + ChaseCon.CurrentCar + "^7) Speed: ^1" + ChaseCon.CompCar.Speed / 146 + " mph", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                        }
                                    }
                                    #endregion

                                    InSim.Send_BTN_CreateButton("^7Distance: ^3" + ChaseDist + " ^7meters", Flags.ButtonStyles.ISB_DARK, 4, 25, 136, 171, 19, Conn.UniqueID, 2, false);

                                    #region ' Timer Bump '
                                    if (Conn.CopPanel == 1)
                                    {
                                        if (Conn.Busted == false)
                                        {
                                            if (Conn.JoinedChase == false)
                                            {
                                                if (Conn.ChaseCondition == 5 == false)
                                                {
                                                    if (Conn.AutoBumpTimer == 0)
                                                    {
                                                        InSim.Send_BTN_CreateButton("^7ENGAGE", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                                    }
                                                    else
                                                    {
                                                        #region ' String Timer '
                                                        string Minutes = "0";
                                                        string Seconds = "0";
                                                        Minutes = "" + (Conn.AutoBumpTimer / 60);
                                                        if ((Conn.AutoBumpTimer - ((Conn.AutoBumpTimer / 60) * 60)) < 10)
                                                        {
                                                            Seconds = "0" + (Conn.AutoBumpTimer - ((Conn.AutoBumpTimer / 60) * 60));
                                                        }
                                                        else
                                                        {
                                                            Seconds = "" + (Conn.AutoBumpTimer - ((Conn.AutoBumpTimer / 60) * 60));
                                                        }
                                                        #endregion
                                                        InSim.Send_BTN_CreateButton("^8" + Minutes + ":" + Seconds, Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                                    }
                                                }
                                                else
                                                {
                                                    InSim.Send_BTN_CreateButton("^8ENGAGE", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                                }
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^8ENGAGE", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        else
                                        {
                                            /*if (Conn.AutoBumpTimer == 0)
                                            {*/
                                            InSim.Send_BTN_CreateButton("^7BUSTED", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                            /*}
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^8BUSTED", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                            }*/
                                        }

                                        InSim.Send_BTN_CreateButton("^7DISENGAGE", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 12, 140, 184, 21, Conn.UniqueID, 2, false);

                                    }
                                    else if (Conn.CopPanel == 0)
                                    {
                                        if (Conn.Busted == false)
                                        {
                                            if (Conn.JoinedChase == false)
                                            {
                                                if (Conn.ChaseCondition == 5 == false)
                                                {
                                                    if (Conn.AutoBumpTimer == 0)
                                                    {
                                                        InSim.Send_BTN_CreateButton("^8ENGAGE", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                                    }
                                                    else
                                                    {
                                                        #region ' String Timer '
                                                        string Minutes = "0";
                                                        string Seconds = "0";
                                                        Minutes = "" + (Conn.AutoBumpTimer / 60);
                                                        if ((Conn.AutoBumpTimer - ((Conn.AutoBumpTimer / 60) * 60)) < 10)
                                                        {
                                                            Seconds = "0" + (Conn.AutoBumpTimer - ((Conn.AutoBumpTimer / 60) * 60));
                                                        }
                                                        else
                                                        {
                                                            Seconds = "" + (Conn.AutoBumpTimer - ((Conn.AutoBumpTimer / 60) * 60));
                                                        }
                                                        #endregion
                                                        InSim.Send_BTN_CreateButton("^8" + Minutes + ":" + Seconds, Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                                    }
                                                }
                                                else
                                                {
                                                    InSim.Send_BTN_CreateButton("^8ENGAGE", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                                }
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^8ENGAGE", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_BTN_CreateButton("^8BUSTED", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                        }
                                        InSim.Send_BTN_CreateButton("^8DISENGAGE", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 21, Conn.UniqueID, 2, false);
                                    }

                                    #endregion
                                }
                            }
                        }

                        #endregion

                        #region ' Timers '

                        #region ' Pull Over Message '
                        if (Conn.IsSuspect == true)
                        {
                            if (Conn.PullOvrMsg == 0 == false)
                            {
                                if (Conn.PullOvrMsg > 0)
                                {
                                    Conn.PullOvrMsg -= 1;
                                }
                                if (Conn.PullOvrMsg == 0)
                                {
                                    MsgAll(Conn.PlayerName + " ^4‹-- ^1Pull Over Now!");
                                    Conn.PullOvrMsg = 30;
                                }
                            }
                        }
                        #endregion

                        #region ' Clear Warning '

                        if (Conn.ModerationWarn == 0 == false)
                        {
                            if (Conn.ModerationWarn > 0)
                            {
                                Conn.ModerationWarn -= 1;
                            }

                            if (Conn.ModerationWarn == 0)
                            {
                                DeleteBTN(10, Conn.UniqueID);
                            }
                        }

                        #endregion

                        #region ' Busted Timer '

                        if (Conn.BustedTimer == 5 == false)
                        {
                            if (Conn.BustedTimer > 0)
                            {
                                Conn.BustedTimer += 1;
                            }
                        }

                        #endregion

                        #region ' Bump Timer '

                        if (Conn.AutoBumpTimer == 0 == false)
                        {
                            if (Conn.InChaseProgress == true && Conn.Busted == false)
                            {
                                Conn.AutoBumpTimer -= 1;
                            }
                        }

                        #endregion

                        #region ' Bank Bonus Timer '

                        if (Conn.BankBonusTimer == 0 == false)
                        {
                            if (Conn.BankBonusTimer > 0)
                            {
                                if (Conn.IsAFK == false)
                                {
                                    Conn.BankBonusTimer -= 1;
                                }
                            }
                            if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true && Conn.InBank == true)
                            {
                                #region ' String Timer '
                                string Minutes = "0";
                                string Seconds = "0";

                                Minutes = "" + (Conn.BankBonusTimer / 60);
                                if ((Conn.BankBonusTimer - ((Conn.BankBonusTimer / 60) * 60)) < 10)
                                {
                                    Seconds = "0" + (Conn.BankBonusTimer - ((Conn.BankBonusTimer / 60) * 60));
                                }
                                else
                                {
                                    Seconds = "" + (Conn.BankBonusTimer - ((Conn.BankBonusTimer / 60) * 60));
                                }
                                #endregion
                                if (Conn.IsAFK == false)
                                {
                                    if (Conn.BankBalance > 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Time until Bonus ^1" + Minutes + ":" + Seconds + " ^7left", Flags.ButtonStyles.ISB_LEFT, 4, 40, 69, 54, 115, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^7You don't have any Bank Balance on Account!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 69, 54, 115, Conn.UniqueID, 2, false);
                                    }
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^7You are AFK and its Disabled.", Flags.ButtonStyles.ISB_LEFT, 4, 40, 69, 54, 115, Conn.UniqueID, 2, false);
                                }
                            }
                            if (Conn.BankBonusTimer == 0)
                            {
                                if (Conn.BankBalance > 0)
                                {
                                    Conn.BankBonus += Conn.BankBalance * 1 / 100;
                                    MsgAll("^9 " + Conn.PlayerName + " has received ^2$" + Conn.BankBonus + " ^7from Bank Bonus!");
                                    Conn.BankBalance += Conn.BankBonus;
                                    Conn.BankBonusTimer = 3600;
                                }
                            }
                        }

                        #endregion

                        #region ' Check AFK Tick '

                        if (Conn.BankBalance > 0)
                        {
                            #region ' Tick '
                            if (kmh < 2)
                            {
                                if (Conn.AFKTick == 60 == false)
                                {
                                    Conn.AFKTick += 1;
                                }
                            }
                            else
                            {
                                if (Conn.AFKTick > 0)
                                {
                                    Conn.AFKTick = 0;
                                }
                                if (Conn.IsAFK == true)
                                {
                                    Conn.IsAFK = false;
                                }
                            }
                            #endregion

                            #region ' Warn '
                            if (Conn.AFKTick == 60)
                            {
                                if (Conn.IsAFK == false)
                                {
                                    MsgPly("^9 WARNING: ^7You are ^1AFK ^7(1 Minute)", Conn.UniqueID);
                                    MsgPly("^9 Bank bonus is disabled whilst your ^1AFK", Conn.UniqueID);
                                    Conn.IsAFK = true;
                                }
                            }
                            #endregion
                        }

                        #endregion

                        #region ' Clear Filters '

                        if (Conn.SwearTime == 0 == false)
                        {
                            if (Conn.SwearTime > 0)
                            {
                                Conn.SwearTime -= 1;
                            }
                            if (Conn.SwearTime == 0)
                            {
                                Conn.Swear = 0;
                            }
                        }

                        if (Conn.SpamTime == 0 == false)
                        {
                            if (Conn.SpamTime > 0)
                            {
                                Conn.SpamTime -= 1;
                            }
                            if (Conn.SpamTime == 0)
                            {
                                Conn.Spam = 0;
                            }
                        }

                        #endregion

                        #region ' Clear Penalty '
                        if (Conn.Penalty == 0 == false)
                        {
                            if (Conn.Penalty > 0)
                            {
                                Conn.Penalty -= 1;
                            }
                            if (Conn.Penalty == 0)
                            {
                                ClearPen(Conn.Username);
                            }
                        }
                        #endregion

                        #region ' Clear Command Buffer '

                        if (Conn.WaitCMD == 0 == false)
                        {
                            if (Conn.WaitCMD > 0)
                            {
                                Conn.WaitCMD -= 1;
                            }
                        }

                        #endregion

                        #region ' Clear Speed Ahead '

                        if (Conn.StreetSign == 0 == false)
                        {
                            if (Conn.StreetSign > 0)
                            {
                                Conn.StreetSign -= 1;
                            }
                            if (Conn.StreetSign == 0)
                            {
                                DeleteBTN(11, Conn.UniqueID);
                                DeleteBTN(12, Conn.UniqueID);
                                DeleteBTN(13, Conn.UniqueID);
                            }
                        }

                        #endregion

                        #region ' Load Button Buffer '

                        if (Conn.WaitIntrfc == 0 == false)
                        {
                            if (Conn.WaitIntrfc > 0)
                            {
                                Conn.WaitIntrfc -= 1;
                            }
                        }

                        #endregion

                        #endregion
                    }
                }

                #region ' Staging '
                if (Stage == 0)
                {
                    Stage = 1;
                }
                else
                {
                    Stage = 0;
                }
                #endregion

                
            }
            catch { }
        }

        // Check User 5000ms (5 Second)
        private void CheckUser_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                foreach (clsConnection C in Connections)
                {
                    if ((C.PlayerName == HostName && C.UniqueID == 0) == false)
                    {
                        #region ' CompCar Detailzzz '
                        int sayi;
                        var kmh = C.CompCar.Speed / 91;
                        var mph = C.CompCar.Speed / 146;
                        var direction = C.CompCar.Direction / 180;
                        var node = C.CompCar.Node;
                        var pathx = C.CompCar.X / 196608;
                        var pathy = C.CompCar.Y / 196608;
                        var pathz = C.CompCar.Z / 98304;
                        var angle = C.CompCar.AngVel / 30;

                        #endregion

                        #region ' Cash Payout '
                        if (TrackName == "KY1")
                        {
                            if (C.CurrentCar == "UF1")
                            {
                                if (kmh > 29)
                                {
                                    Random uf1 = new Random();
                                    sayi = uf1.Next(10, 30);
                                    C.Cash += sayi;
                                    if (C.Cash > -1)
                                    {
                                        InSim.Send_BTN_CreateButton(" ^7" + sayi + "+^2$", Flags.ButtonStyles.ISB_C4, 4, 25, 0, 134, 2, C.UniqueID, 2, true);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton(" ^5" + sayi + "^1$", Flags.ButtonStyles.ISB_C4, 4, 25, 0, 134, 2, C.UniqueID, 2, true);
                                    }
                                }
                            }

                            if (C.CurrentCar == "XFG" || C.CurrentCar == "XRG" || C.CurrentCar == "LX4" || C.CurrentCar == "LX6" || C.CurrentCar == "RB4" || C.CurrentCar == "FXO" || C.CurrentCar == "XRT" || C.CurrentCar == "RAC" || C.CurrentCar == "FZ5" || C.CurrentCar == "UFR" || C.CurrentCar == "XFR" || C.CurrentCar == "FXR" || C.CurrentCar == "XRR" || C.CurrentCar == "FZR" || C.CurrentCar == "MRT" || C.CurrentCar == "FBM" || C.CurrentCar == "FOX" || C.CurrentCar == "FO8")
                            {
                                if (kmh > 29)
                                {
                                    Random ALL = new Random();
                                    sayi = ALL.Next(4, 10);
                                    C.Cash += sayi;
                                    if (C.Cash > -1)
                                    {
                                        InSim.Send_BTN_CreateButton(" ^7" + sayi + "+^2$", Flags.ButtonStyles.ISB_C4, 4, 25, 0, 134, 2, C.UniqueID, 2, true);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton(" ^5" + sayi + "+^1$", Flags.ButtonStyles.ISB_C4, 4, 25, 0, 134, 2, C.UniqueID, 2, true);
                                    }
                                }
                            }


                            if (C.CurrentCar == "BF1")
                            {
                                if (kmh > 1)
                                {
                                    Random BF1 = new Random();
                                    sayi = BF1.Next(250, 500);
                                    C.Cash += sayi;
                                    if (C.Cash > -1)
                                    {
                                        InSim.Send_BTN_CreateButton(" ^7" + sayi + "+^2$", Flags.ButtonStyles.ISB_C4, 4, 25, 0, 134, 2, C.UniqueID, 2, true);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton(" ^5" + sayi + "+^1$", Flags.ButtonStyles.ISB_C4, 4, 25, 0, 134, 2, C.UniqueID, 2, true);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (C.CurrentCar == "UF1")
                            {
                                if (kmh > 29)
                                {
                                    Random uf1 = new Random();
                                    sayi = uf1.Next(5, 15);
                                    C.Cash += sayi;
                                    if (C.Cash > -1)
                                    {
                                        InSim.Send_BTN_CreateButton(" ^7" + sayi + "+^2$", Flags.ButtonStyles.ISB_C4, 4, 25, 0, 134, 2, C.UniqueID, 2, true);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton(" ^5" + sayi + "+^1$", Flags.ButtonStyles.ISB_C4, 4, 25, 0, 134, 2, C.UniqueID, 2, true);
                                    }
                                }
                            }

                            if (C.CurrentCar == "XFG" || C.CurrentCar == "XRG" || C.CurrentCar == "LX4" || C.CurrentCar == "LX6" || C.CurrentCar == "RB4" || C.CurrentCar == "FXO" || C.CurrentCar == "XRT" || C.CurrentCar == "RAC" || C.CurrentCar == "FZ5" || C.CurrentCar == "UFR" || C.CurrentCar == "XFR" || C.CurrentCar == "FXR" || C.CurrentCar == "XRR" || C.CurrentCar == "FZR" || C.CurrentCar == "MRT" || C.CurrentCar == "FBM" || C.CurrentCar == "FOX" || C.CurrentCar == "FO8")
                            {
                                if (kmh > 29)
                                {
                                    Random ALL = new Random();
                                    sayi = ALL.Next(2, 5);
                                    C.Cash += sayi;
                                    if (C.Cash > -1)
                                    {
                                        InSim.Send_BTN_CreateButton(" ^7" + sayi + "+^2$", Flags.ButtonStyles.ISB_C4, 4, 25, 0, 134, 2, C.UniqueID, 2, true);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton(" ^5" + sayi + "+^1$", Flags.ButtonStyles.ISB_C4, 4, 25, 0, 134, 2, C.UniqueID, 2, true);
                                    }
                                }
                            }


                            if (C.CurrentCar == "BF1")
                            {
                                if (kmh > 1)
                                {
                                    Random BF1 = new Random();
                                    sayi = BF1.Next(250, 500);
                                    C.Cash += sayi;
                                    if (C.Cash > -1)
                                    {
                                        InSim.Send_BTN_CreateButton(" ^7" + sayi + "+^2$", Flags.ButtonStyles.ISB_C4, 4, 25, 0, 134, 2, C.UniqueID, 2, true);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton(" ^5" + sayi + "+^1$", Flags.ButtonStyles.ISB_C4, 4, 25, 0, 134, 2, C.UniqueID, 2, true);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region ' Health Distance '

                        if (C.HealthDist > 2500)
                        {
                            C.TotalHealth += 1;
                            C.HealthDist = 0;
                        }

                        if (C.TotalHealth > 90)
                        {
                            if (C.HealthWarn == 0)
                            {
                                MsgPly("^9 Warning: Your Car Demage is reached ^6" + C.TotalHealth + "%", C.UniqueID);
                                MsgPly("^9 May cause specting because of doctors checkup.", C.UniqueID);
                                C.HealthWarn = 1;
                            }
                        }

                        if (C.TotalHealth > 95)
                        {
                            int RandomFines = new Random().Next(500, 800);
                            MsgAll("^9 " + C.PlayerName + " was charged ^1$" + RandomFines + " ^7from doctors mechanic.");
                            MsgPly("^9 You are spected because of your Car Demage.", C.UniqueID);

                            C.Cash -= RandomFines;
                            C.TotalHealth = 0;
                            SpecID(C.PlayerName);
                            SpecID(C.Username);
                            C.HealthWarn = 0;
                        }

                        #endregion

                        #region ' PlayerHost Check '

                        if (C.UniqueID == 0 && C.PlayerName == HostName)
                        {
                            C.Cash = 0;
                            C.BankBalance = 0;
                            C.Cars = "UF1";
                            C.TotalHealth = 100;
                            C.TotalDistance = 0;
                            C.FailCon = 1;
                        }

                        #endregion

                        #region ' InSim Interface '
                       
                                #region ' Cash Inteface '
                        if (TrackName == "KY1")
                        {
                            if (C.Cash > -1)
                            {
                                    InSim.Send_BTN_CreateButton("^3Money: ^2$" + string.Format("{0:n0}", C.Cash), Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_LEFT, 4, 25, 0, 124, 200, C.UniqueID, 2, true);
                            }
                            else
                            {
                                InSim.Send_BTN_CreateButton("^3Money: ^1$" + string.Format("{0:n0}", C.Cash), Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_LEFT, 4, 25, 0, 124, 200, C.UniqueID, 2, true);
                            }
                        }
                        else
                        {
                            if (C.Cash > -1)
                            {
                                InSim.Send_BTN_CreateButton("^3Money: ^2$" + string.Format("{0:n0}", C.Cash), Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_LEFT, 4, 25, 0, 124, 200, C.UniqueID, 2, true);
                            }
                            else
                            {
                                InSim.Send_BTN_CreateButton("^3Money: ^1$" + string.Format("{0:n0}", C.Cash), Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_LEFT, 4, 25, 0, 124, 200, C.UniqueID, 2, true);
                            }
                        }
                                #endregion

                                #region ' Website/ChaseCon/Towed '
                                if (C.InTowProgress == false && C.InChaseProgress == false)
                                {
                                    if (C.IsSuspect == false && C.IsBeingTowed == false && C.IsBeingBusted == false)
                                    {
                                        if (C.JobToHouse1 == false && C.JobToHouse2 == false && C.JobToHouse3 == false && C.JobToSchool == false)
                                        {
                                            InSim.Send_BTN_CreateButton("Current Car: " + C.CurrentCar + " - Distance: " + C.TotalDistance.ToString("000:000:000") + " ^J‡q" + " - Car Demage: ^3" + C.TotalHealth + " %", Flags.ButtonStyles.ISB_DARK, 4, 60, 0, 54, 6, C.UniqueID, 2, true);
                                        }
                                    }
                                    else
                                    {
                                        if (C.IsSuspect == true)
                                        {
                                            if (C.ChaseCondition == 1)
                                            {
                                                InSim.Send_BTN_CreateButton("^4^S¡ï" + "^7^J™" + "^7^J™" + "^7^J™" + "^7^J™", Flags.ButtonStyles.ISB_DARK, 4, 60, 0, 54, 6, C.UniqueID, 2, false);
                                            }
                                            else if (C.ChaseCondition == 2)
                                            {
                                                InSim.Send_BTN_CreateButton("^4^S¡ï" + "^4^S¡ï" + "^7^J™" + "^7^J™" + "^7^J™", Flags.ButtonStyles.ISB_DARK, 4, 60, 0, 54, 6, C.UniqueID, 2, false);
                                            }
                                            else if (C.ChaseCondition == 3)
                                            {
                                                InSim.Send_BTN_CreateButton("^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï" + "^7^J™" + "^7^J™", Flags.ButtonStyles.ISB_DARK, 4, 60, 0, 54, 6, C.UniqueID, 2, false);
                                            }
                                            else if (C.ChaseCondition == 4)
                                            {
                                                InSim.Send_BTN_CreateButton("^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï" + "^7^J™", Flags.ButtonStyles.ISB_DARK, 4, 60, 0, 54, 6, C.UniqueID, 2, false);
                                            }
                                            else if (C.ChaseCondition == 5)
                                            {
                                                InSim.Send_BTN_CreateButton("^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï", Flags.ButtonStyles.ISB_DARK, 4, 60, 0, 54, 6, C.UniqueID, 2, false);
                                            }
                                        }
                                        else if (C.IsBeingBusted == true)
                                        {
                                            InSim.Send_BTN_CreateButton("^7You are Busted!", Flags.ButtonStyles.ISB_DARK, 4, 60, 0, 54, 6, C.UniqueID, 2, false);
                                        }
                                        if (C.IsBeingTowed == true)
                                        {
                                            InSim.Send_BTN_CreateButton("^7You are being Towed!", Flags.ButtonStyles.ISB_DARK, 4, 60, 0, 54, 6, C.UniqueID, 2, false);
                                        }
                                    }
                                }
                                else
                                {
                                    if (C.InTowProgress == true)
                                    {
                                        clsConnection TowCon = Connections[GetConnIdx(C.Towee)];
                                        InSim.Send_BTN_CreateButton("^7Tow: " + TowCon.PlayerName, Flags.ButtonStyles.ISB_DARK, 4, 60, 0, 54, 6, C.UniqueID, 2, true);
                                    }
                                    if (C.InChaseProgress == true)
                                    {
                                        clsConnection ChaseCon = Connections[GetConnIdx(C.Chasee)];
                                        InSim.Send_BTN_CreateButton("^7Chase: " + ChaseCon.PlayerName, Flags.ButtonStyles.ISB_DARK, 4, 60, 0, 54, 6, C.UniqueID, 2, true);
                                    }
                                }
                                #endregion
                           
                                #region ' Timers '
                                if (TrackName == "KY1")
                                {

                                    InSim.Send_BTN_CreateButton("^3Cash x2", Flags.ButtonStyles.ISB_DARK, 4, 10, 0, 114, 3, C.UniqueID, 2, true);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^3Cash x1", Flags.ButtonStyles.ISB_DARK, 4, 10, 0, 114, 3, C.UniqueID, 2, true);
                                }
                                #endregion

                                #region ' Bonus Meter '

                                if (C.TotalBonusDone == 0)
                                {
                                    InSim.Send_BTN_CreateButton("Bonus: ^3 $300 ^7% ^3" + C.BonusDistance / 400, Flags.ButtonStyles.ISB_DARK, 4, 18, 0, 36, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 1)
                                {
                                    InSim.Send_BTN_CreateButton("Bonus: ^3 ^7% ^3" + C.BonusDistance / 800, Flags.ButtonStyles.ISB_DARK, 4, 12, 0, 42, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 2)
                                {
                                    InSim.Send_BTN_CreateButton("Bonus: ^3 $900 ^7% ^3" + C.BonusDistance / 600, Flags.ButtonStyles.ISB_DARK, 4, 12, 0, 42, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 3)
                                {
                                    InSim.Send_BTN_CreateButton("Bonus: ^3 $1100 ^7% ^3" + C.BonusDistance / 1200, Flags.ButtonStyles.ISB_DARK, 4, 12, 0, 42, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 4)
                                {
                                    InSim.Send_BTN_CreateButton("Bonus: ^3 $1300 ^7% ^3" + C.BonusDistance / 1600, Flags.ButtonStyles.ISB_DARK, 4, 12, 0, 42, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 5)
                                {
                                    InSim.Send_BTN_CreateButton("Bonus: ^3 $1500 ^7% ^3" + C.BonusDistance / 2000, Flags.ButtonStyles.ISB_DARK, 4, 12, 0, 42, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 6)
                                {
                                    InSim.Send_BTN_CreateButton("Bonus: ^3 $1700 ^7% ^3" + C.BonusDistance / 2600, Flags.ButtonStyles.ISB_DARK, 4, 12, 0, 42, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 7)
                                {
                                    InSim.Send_BTN_CreateButton("Bonus: ^3 $1900 ^7% ^3" + C.BonusDistance / 3200, Flags.ButtonStyles.ISB_DARK, 4, 12, 0, 42, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 8)
                                {
                                    InSim.Send_BTN_CreateButton("Bonus: ^3 $2100 ^7% ^3" + C.BonusDistance / 4000, Flags.ButtonStyles.ISB_DARK, 4, 12, 0, 42, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 9)
                                {
                                    InSim.Send_BTN_CreateButton("Bonus: ^3 $2300 ^7% ^3" + C.BonusDistance / 5400, Flags.ButtonStyles.ISB_DARK, 4, 12, 0, 42, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 10)
                                {
                                    InSim.Send_BTN_CreateButton("Bonus: ^3 $2900 ^7% ^3" + C.BonusDistance / 6600, Flags.ButtonStyles.ISB_DARK, 4, 12, 0, 42, 5, C.UniqueID, 2, true);
                                }

                                #endregion

                                #region ' SOS '

                                if (C.CompCar.Speed / 91 < 2 && C.InAdminMenu == false && C.DisplaysOpen == false && C.InModerationMenu == 0 && C.InFineMenu == false)
                                {
                                    InSim.Send_BTN_CreateButton("^1› SOS ‹", Flags.ButtonStyles.ISB_C4 | Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 5, 16, 16, 183, 25, C.UniqueID, 2, false);
                                }
                                else
                                {
                                    DeleteBTN(25, C.UniqueID);
                                }

                                #endregion

                                if (Stage == 0)
                                {
                                    if (TrackName == "KY1")
                                    {
                                        InSim.Send_BTN_CreateButton("^1[M-TeCh] ^7Bonus Night" + " " + Website, Flags.ButtonStyles.ISB_DARK, 4, 40, 0, 149, 1, C.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton(CruiseName + " " + Website, Flags.ButtonStyles.ISB_DARK, 4, 40, 0, 149, 1, C.UniqueID, 2, false);
                                    }
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^7Team Speak: "+TSIP, Flags.ButtonStyles.ISB_DARK, 4, 40, 0, 149, 1, C.UniqueID, 2, false);
                                }
                        #endregion

                        #region ' Distance Bonus '

                        if (C.BonusDistance > 40000 && C.TotalBonusDone == 0)
                        {
                            MsgAll("^9 " + C.PlayerName + " earned ^2$300 ^9for reached 4 km!");
                            C.Cash += 300;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 60000 && C.TotalBonusDone == 1)
                        {
                            MsgAll("^9 " + C.PlayerName + " earned ^2$500 ^9for reached 6 km!");
                            C.Cash += 500;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 80000 && C.TotalBonusDone == 2)
                        {
                            MsgAll("^9 " + C.PlayerName + " earned ^2$900 ^9for reached 8 km!");
                            C.Cash += 900;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 120000 && C.TotalBonusDone == 3)
                        {
                            MsgAll("^9 " + C.PlayerName + " earned ^2$1100 ^9for reached 12 km!");
                            C.Cash += 1100;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 160000 && C.TotalBonusDone == 4)
                        {
                            MsgAll("^9 " + C.PlayerName + " earned ^2$1300 ^9for reached 16 km!");
                            C.Cash += 1300;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 200000 && C.TotalBonusDone == 5)
                        {
                            MsgAll("^9 " + C.PlayerName + " earned ^2$1500 ^9for reached 20 km!");
                            C.Cash += 1500;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 260000 && C.TotalBonusDone == 6)
                        {
                            MsgAll("^9 " + C.PlayerName + " earned ^2$1700 ^9for reached 26 km!");
                            C.Cash += 1700;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 320000 && C.TotalBonusDone == 7)
                        {
                            MsgAll("^9 " + C.PlayerName + " earned ^2$1900 ^9for reached 32 km!");
                            C.Cash += 1900;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 400000 && C.TotalBonusDone == 8)
                        {
                            MsgAll("^9 " + C.PlayerName + " earned ^2$33000 ^9for reached 40 km!");
                            C.Cash += 33000;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 540000 && C.TotalBonusDone == 9)
                        {
                            MsgAll("^9 " + C.PlayerName + " earned ^2$45000 ^9for reached 54 km!");
                            C.Cash += 45000;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 660000 && C.TotalBonusDone == 10)
                        {
                            MsgAll("^9 " + C.PlayerName + " earned ^2$55000 ^9for reached 66 km!");
                            MsgPly("^9 Your distance bonus is now reseted to Lvl. 1", C.UniqueID);
                            C.Cash += 55000;
                            C.BonusDistance = 0;
                            C.TotalBonusDone = 0;
                        }

                        #endregion

                        
                    }
                }
            }
            catch { }
        }

        #endregion
    }
}