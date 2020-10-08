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
    public partial class Form1 //Chat commands
    {
        public struct CommandList
        {
            public MethodInfo MethodInf;
            public CommandAttribute CommandArg;
        }
        public List<CommandList> Commands = new List<CommandList>();
        public class CommandAttribute : Attribute
        {
            public string Command;
            public string Syntax;
            public string Description;
            public CommandAttribute(string command, string syntax, string desc)
            {
                Command = command;
                Syntax = syntax;
                Description = desc;
            }
            public CommandAttribute(string command, string syntax)
            {
                Command = command;
                Syntax = syntax;
                Description = "";
            }
        }

        // Your Code Here
        #region ' InSim Settings '
        [Command("test", "test")]
        public void test(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (Connections[GetConnIdx(MSO.UCID)].Username == "skywatcher122")
            {
                /*MsgPly(HostName, MSO.UCID);
                if (GameMode == 0)
                {
                    MsgPly("> " + GameMode + " DEMO", MSO.UCID);
                }
                if (GameMode == 1)
                {
                    MsgPly("> " + GameMode + " S1", MSO.UCID);
                }
                if (GameMode == 2)
                {
                    MsgPly("> " + GameMode + " S2", MSO.UCID);
                }

                clsConnection Conn = Connections[GetConnIdx(MSO.UCID)];

                MsgPly("^9 Current Car: " + Conn.CurrentCar, MSO.UCID);
                
                if (Conn.InGameIntrfc == 0)
                {
                    Conn.InGameIntrfc = 1;
                }
                else
                {
                    Conn.InGameIntrfc = 0;
                }

                Conn.JobToHouse2 = true;
                Conn.JobFromShop = true;*/

                //MsgPly("> Length " + Connections[GetConnIdx(MSO.UCID)].Cars.Length, MSO.UCID);
                //Connections[GetConnIdx(MSO.UCID)].InTowProgress = true;

                clsConnection Conn = Connections[GetConnIdx(MSO.UCID)];
                //MsgAll(Conn.SendUSRN1);
                //MsgAll(Conn.SendUSRN2);
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 4;
            /*MsgPly("> " + Connections[GetConnIdx(MSO.UCID)].Rentee, MSO.UCID);
            MsgPly("> " + Connections[GetConnIdx(MSO.UCID)].Renter, MSO.UCID);

            MsgPly("> " + Connections[GetConnIdx(MSO.UCID)].Rented, MSO.UCID);

            MsgPly("> " + Connections[GetConnIdx(MSO.UCID)].Renting, MSO.UCID);*/
        }


        #endregion

        #region ' Player Help Command '

        [Command("help", "help")]
        public void help(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length == 1)
            {
                InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 90, 59, 17, 28, 118, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton(CruiseName + " BETA ^7Help Menu", Flags.ButtonStyles.ISB_DARK, 6, 50, 19, 30, 101, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("!remdemage ^7-Remove your Car Demage ^1$1000!", Flags.ButtonStyles.ISB_LEFT, 5, 55, 25, 30, 102, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("!kminfo ^7-To see km's for cars!", Flags.ButtonStyles.ISB_LEFT, 5, 55, 30, 30, 103, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("!send X Y ^7- Send cash to the username", Flags.ButtonStyles.ISB_LEFT, 5, 55, 35, 30, 104, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("!cops ^7- See all Online Cops", Flags.ButtonStyles.ISB_LEFT, 5, 55, 40, 30, 105, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("!towtruck ^7- See all Online TowTruck", Flags.ButtonStyles.ISB_LEFT, 5, 55, 45, 30, 106, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("!members ^7- See all Online Members", Flags.ButtonStyles.ISB_LEFT, 5, 55, 50, 30, 107, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("!admins ^7- See all Online Admins", Flags.ButtonStyles.ISB_LEFT, 5, 55, 55, 30, 108, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("!cophelp ^7- See all command(For Cops)", Flags.ButtonStyles.ISB_LEFT, 5, 55, 60, 30, 109, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("!towhelp ^7- See all command(For Tow's)", Flags.ButtonStyles.ISB_LEFT, 5, 55, 65, 30, 110, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("!towrequest ^7- Call a Tow Request!", Flags.ButtonStyles.ISB_LEFT, 5, 55, 70, 30, 111, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("!tags ^7- To see tags!", Flags.ButtonStyles.ISB_LEFT, 5, 55, 75, 30, 112, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("!buy X ^7- Buy a Car see (!prices)", Flags.ButtonStyles.ISB_LEFT, 5, 55, 80, 30, 113, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("!sell X ^7- Sell a Car see (!prices)", Flags.ButtonStyles.ISB_LEFT, 5, 55, 85, 30, 114, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("!pm X Y ^7- Send Private Message to the username", Flags.ButtonStyles.ISB_LEFT, 5, 55, 90, 30, 115, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^1CLOSE [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 13, 97, 50, 121, Conn.UniqueID, 2, false);
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 4;
        }

        [Command("cops", "cops")]
        public void cops(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            bool Found = false;

            foreach (clsConnection i in Connections)
            {
                if (i.IsOfficer == true && i.CanBeOfficer == 1)
                {
                    Found = true;
                    MsgPly("^9 Officer: ^7" + i.PlayerName + " (" + i.Username + ") ^7- ^2ON-DUTY", MSO.UCID);
                }
                if (i.IsOfficer == false && i.CanBeOfficer == 1)
                {
                    Found = true;
                    MsgPly("^9 Officer: ^7" + i.PlayerName + " (" + i.Username + ") ^7- ^1OFF-DUTY", MSO.UCID);
                }

                if (i.IsCadet == true && i.CanBeCadet == 1)
                {
                    Found = true;
                    MsgPly("^9 Cadet: ^7" + i.PlayerName + " (" + i.Username + ") ^7- ^2ON-DUTY", MSO.UCID);
                }
                if (i.IsCadet == false && i.CanBeCadet == 1)
                {
                    Found = true;
                    MsgPly("^9 Cadet: ^7" + i.PlayerName + " (" + i.Username + ") ^7- ^1OFF-DUTY", MSO.UCID);
                }
            }

            if (Found == false)
            {
                MsgPly("^9 There are no currently Cops online", MSO.UCID);
            }

            Conn.WaitCMD = 4;
        }

        [Command("towtruck", "towtruck")]
        public void towtruck(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            bool Found = false;

            foreach (clsConnection i in Connections)
            {
                if (i.IsTowTruck == true && i.CanBeTowTruck == 1)
                {
                    Found = true;
                    MsgPly("^9 Tow Truck: ^7" + i.PlayerName + " (" + i.Username + ") ^7- ^2ON-DUTY", MSO.UCID);
                }
                if (i.IsTowTruck == false && i.CanBeTowTruck == 1)
                {
                    Found = true;
                    MsgPly("^9 Tow Truck: ^7" + i.PlayerName + " (" + i.Username + ") ^7- ^1OFF-DUTY", MSO.UCID);
                }
            }

            if (Found == false)
            {
                MsgPly("^9 There are no currently TowTrucks online", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("members", "members")]
        public void members(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            bool Found = false;

            foreach (clsConnection i in Connections)
            {
                if (i.IsModerator == 1)
                {
                    Found = true;
                    MsgPly("^9 Member: ^7" + i.PlayerName + " (" + i.Username + ") ^7- ^2ONLINE", MSO.UCID);
                }
            }

            if (Found == false)
            {
                MsgPly("^9 There are no currently Members online", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("admins", "admins")]
        public void admins(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            bool Found = false;

            foreach (clsConnection i in Connections)
            {
                if (i.IsAdmin == 1 && i.IsSuperAdmin == 1)
                {
                    Found = true;
                    MsgPly("^9 Admin: ^7" + i.PlayerName + " (" + i.Username + ") ^7- ^2ONLINE", MSO.UCID);
                }
            }

            if (Found == false)
            {
                MsgPly("^9 There are no currently Admins online", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("demage", "demage")]
        public void dm(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length == 1)
            {
                var Conn = Connections[GetConnIdx(MSO.UCID)];
                MsgPly("^9Your Car Demage: ", MSO.UCID);
                MsgPly("Car Demage: ^1" + Conn.TotalHealth + " %", MSO.UCID);


            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 4;
        }
        [Command("bonus", "bonus")]
        public void bs(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length == 1)
            {
                var Conn = Connections[GetConnIdx(MSO.UCID)];
                MsgPly("^9Your Bonus Stats: ", MSO.UCID);
                MsgPly("Bonus level: ^1" + Conn.TotalBonusDone, MSO.UCID);



            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 4;
        }
        [Command("tags", "tags")]
        public void tagi(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length == 1)
            {
                MsgPly(CruiseName + " ^7Tag List:", MSO.UCID);

                MsgPly("^2Officer tag ^7- " + OfficerTag, MSO.UCID);
                MsgPly("^2Cadet tag ^7- " + CadetTag, MSO.UCID);
                MsgPly("^2TowTruck tag ^7- " + TowTruckTag, MSO.UCID);
            }
        }
        [Command("cars", "cars")]
        public void cars(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length == 1)
            {
                MsgPly("^9Your Cars: ", MSO.UCID);
                MsgPly(Connections[GetConnIdx(MSO.UCID)].Cars, MSO.UCID);


            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 4;
        }


        [Command("cophelp", "cophelp")]
        public void cophelp(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length == 1)
            {
                if (Connections[GetConnIdx(MSO.UCID)].CanBeOfficer == 1 || Connections[GetConnIdx(MSO.UCID)].CanBeCadet == 1)
                {
                    MsgPly("^9 Cop Help Command: ", MSO.UCID);
                    MsgPly("^2!chase  ^7- start a chase or backup", MSO.UCID);
                    MsgPly("^2!disengage ^7- stops the chase on suspect", MSO.UCID);
                    MsgPly("^2!busted ^7- busted suspect when 5 seconds", MSO.UCID);
                    MsgPly("^2!backup ^7- Call a backup (only works cond.2 upwards)", MSO.UCID);
                    MsgPly("^2!cc <message> ^7- Cop Chat", MSO.UCID);
                }
                else
                {
                    MsgPly("^9 You need to be a Cop in this system!", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 4;
        }

        [Command("towhelp", "towhelp")]
        public void towhelp(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length == 1)
            {
                if (Connections[GetConnIdx(MSO.UCID)].CanBeTowTruck == 1)
                {
                    MsgPly("^9 Tow Help Command: ", MSO.UCID);
                    MsgPly("^2!accepttow <username>  ^7- when only call is requested by user!", MSO.UCID);
                    MsgPly("^2!starttow ^7- engage the tow siren!", MSO.UCID);
                    MsgPly("^2!stoptow ^7- stop tow in progress", MSO.UCID);
                }
                else
                {
                    MsgPly("^9 You need to be a Tow Truck in this system!", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 4;
        }
        [Command("kminfo", "kminfo")]
        public void kminfo(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {

            if (StrMsg.Length == 1)
            {
                MsgPly("^9 LX4 - 400 km", MSO.UCID);
                MsgPly("^9 LX6 - 800 km", MSO.UCID);

                MsgPly("^9 RB4 - 900 km", MSO.UCID);
                MsgPly("^9 FXO - 1200 km", MSO.UCID);
                MsgPly("^9 XRT - 1600 km", MSO.UCID);
                MsgPly("^9 RAC - 1800 km", MSO.UCID);
                MsgPly("^9 FZ5 - 2000 km", MSO.UCID);
                MsgPly("^9 UFR - 2200 km", MSO.UCID);

                MsgPly("^9 XFR - 2400 km", MSO.UCID);
                MsgPly("^9 FXR - 3000 km", MSO.UCID);
                MsgPly("^9 XRR - 3500 km", MSO.UCID);
                MsgPly("^9 FZR - 4000 km", MSO.UCID);
                MsgPly("^9 MRT - 1000 km", MSO.UCID);

                MsgPly("^9 FBM - 10 000 km", MSO.UCID);
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 4;
        }
        [Command("remdemage", "remdemage")]
        public void redbull(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            string Car = Connections[GetConnIdx(MSO.UCID)].CurrentCar;

            if (StrMsg.Length == 1)
            {
                MsgPly("^9 You remove your Car Demage for ^1$1000 ", MSO.UCID);
                Conn.TotalHealth = 0;
                Conn.Cash -= 1000;
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 4;
        }

        [Command("report", "report <msg>")]
        public void report(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length > 1)
            {
                MsgPly("^1*^7 Your message will be forwarded to the administrator!", MSO.UCID);
                clsConnection Conn = Connections[GetConnIdx(MSO.UCID)];

                foreach (clsConnection C in Connections)
                {

                    string Message2 = Msg.Remove(0, 7);

                    if ((C.IsAdmin == 1 && C.IsSuperAdmin == 1) && C.UniqueID != MSO.UCID)
                    {
                        InSim.Send_MTC_MessageToConnection("^1*^7 Report by: ^7" + Conn.PlayerName, C.UniqueID, 0);
                        InSim.Send_MTC_MessageToConnection("^1*^7" + Message2, C.UniqueID, 0);
                    }


                }

            }
            else
            {
                InSim.Send_MTC_MessageToConnection("^6»^7 Invalid Command. ^2!help ^7for help.", MSO.UCID, 0);
            }
        }
        [Command("showoff", "showoff")]
        public void showoff(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];

            #region ' Normal Showoff '
            if (StrMsg.Length == 1)
            {
                MsgAll("^9 Showoff: ^7" + Conn.PlayerName + " (" + Conn.Username + ")");
                MsgAll("^9 Cash: ^7$" + string.Format("{0:n0}", Conn.Cash) + " ^7/ ^9Bank Cash: ^7$" + string.Format("{0:n0}", Conn.BankBalance));
                MsgAll("^9 Distance: ^7" + Conn.TotalDistance.ToString("000:000:000") + " kms ^7/ ^9" + Conn.TotalDistance / 1609 + " mi");

                #region ' Car Lines '
                if (Conn.Cars.Length > 52 && Conn.Cars.Length < 84)
                {
                    MsgAll("^9 Cars: ^7" + Conn.Cars.Remove(39, Conn.Cars.Length - 39));
                    MsgAll("^9 ^7" + Conn.Cars.Remove(0, 40));
                }
                else
                {
                    MsgAll("^9 Cars: ^7" + Conn.Cars);
                }
                #endregion
            }
            #endregion

            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("location", "location")]
        public void locate(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];

            if (StrMsg.Length == 1)
            {
                MsgAll("^9 " + Conn.PlayerName + " ^3is located at ^7" + Conn.Location);
                if (Conn.InGame == 1)
                {
                    MsgAll("^3  Positioned at ^7X: " + Conn.CompCar.X / 196608 + " Y: " + Conn.CompCar.Y / 196608);
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("pitlane", "pitlane")]
        public void pitlane(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(MSO.UCID)].Chasee)];
            var TowCon = Connections[GetConnIdx(Connections[GetConnIdx(MSO.UCID)].Towee)];
            if (StrMsg.Length == 1)
            {
                if (Conn.Location.Contains("Spectators") || Conn.Location.Contains("Fix 'N' Repair Station"))
                {
                    MsgPly("^9 You must be in vehicle before you access this command!", MSO.UCID);
                }
                else
                {
                    if (Conn.IsSuspect == false && RobberUCID != MSO.UCID && Conn.IsBeingBusted == false)
                    {
                        if (Conn.IsOfficer == false && Conn.InTowProgress == false && Conn.IsBeingTowed == false)
                        {
                            #region ' Not Officer '
                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                            {
                                if (Conn.Cash > 300)
                                {
                                    MsgAll("^9 " + Conn.PlayerName + " pitlaned for ^1$300^7.");
                                    Conn.Cash -= 300;
                                    PitlaneID(Conn.PlayerName);
                                }
                                else
                                {
                                    MsgPly("^9 Not Enough cash to get pitlaned!", MSO.UCID);
                                }
                            }
                            else
                            {
                                if (Conn.Cash > 750)
                                {
                                    MsgAll("^9 " + Conn.PlayerName + " pitlaned for ^1$750^7.");
                                    Conn.Cash -= 750;
                                    PitlaneID(Conn.PlayerName);
                                }
                                else
                                {
                                    MsgPly("^9 Not Enough cash to get pitlaned!", MSO.UCID);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            if (Conn.IsOfficer == true)
                            {
                                if (Conn.InChaseProgress == true)
                                {
                                    MsgAll("^9 " + Conn.PlayerName + " ^7lost ^3" + ChaseCon.PlayerName + "!");

                                    #region ' Disengage Joined in chase '
                                    if (ChaseCon.CopInChase > 1)
                                    {
                                        if (Conn.JoinedChase == true)
                                        {
                                            Conn.JoinedChase = false;
                                        }
                                        Conn.ChaseCondition = 0;
                                        Conn.Busted = false;
                                        Conn.InChaseProgress = false;
                                        Conn.BustedTimer = 0;
                                        Conn.BumpButton = 0;
                                        Conn.Chasee = -1;
                                        ChaseCon.CopInChase -= 1;

                                        #region ' Connection List '
                                        if (ChaseCon.CopInChase == 1)
                                        {
                                            foreach (clsConnection Con in Connections)
                                            {
                                                if (Con.Chasee == ChaseCon.UniqueID)
                                                {
                                                    if (Con.JoinedChase == true)
                                                    {
                                                        Con.JoinedChase = false;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region ' Disengage '
                                    else if (ChaseCon.CopInChase == 1)
                                    {
                                        AddChaseLimit -= 1;
                                        Conn.AutoBumpTimer = 0;
                                        Conn.BumpButton = 0;
                                        Conn.BustedTimer = 0;
                                        Conn.Chasee = -1;
                                        Conn.Busted = false;
                                        Conn.InChaseProgress = false;
                                        ChaseCon.PullOvrMsg = 0;
                                        ChaseCon.ChaseCondition = 0;
                                        ChaseCon.CopInChase = 0;
                                        ChaseCon.IsSuspect = false;
                                        Conn.ChaseCondition = 0;
                                        CopSirenShutOff();
                                    }
                                    #endregion

                                    MsgAll("^9 " + Conn.PlayerName + " pitlaned.");
                                    PitlaneID(Conn.Username);
                                }
                                else
                                {
                                    MsgAll("^9 " + Conn.PlayerName + " pitlaned.");
                                    PitlaneID(Conn.Username);
                                }
                            }
                            if (Conn.InTowProgress == true)
                            {
                                MsgAll("^9 " + Conn.PlayerName + " stopped towing " + TowCon.PlayerName + "!");
                                TowCon.IsBeingTowed = false;
                                Conn.Towee = -1;
                                Conn.InTowProgress = false;
                                CautionSirenShutOff();

                                MsgAll("^9 " + Conn.PlayerName + " pitlaned.");
                                PitlaneID(Conn.Username);
                            }
                            if (Conn.IsBeingTowed == true)
                            {
                                MsgPly("^9 Can't pitlane whilst being towed.", MSO.UCID);
                            }
                        }
                    }
                    else
                    {
                        MsgPly("^9 Can't pitlane whilst being chased/busted!", MSO.UCID);
                    }
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 3;
        }

        [Command("send", "send <amount>")]
        public void send(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length > 2)
            {
                try
                {
                    int Send = int.Parse(StrMsg[1]);
                    bool SendFound = false;
                    var Conn = Connections[GetConnIdx(MSO.UCID)];
                    #region ' Send Access '
                    if (Connections[GetConnIdx(MSO.UCID)].Username == Msg.Remove(0, 7 + StrMsg[1].Length))
                    {
                        InSim.Send_MTC_MessageToConnection("^9 You can't send money to yourself!", MSO.UCID, 0);
                    }
                    else
                    {
                        foreach (clsConnection C in Connections)
                        {
                            if (C.Username == Msg.Remove(0, 7 + StrMsg[1].Length))
                            {
                                SendFound = true;
                                if (StrMsg[1].Contains("-"))
                                {
                                    InSim.Send_MTC_MessageToConnection("^9 Send Error. Please don't use minus values!", MSO.UCID, 0);
                                }
                                else if (Send > 9001)
                                {
                                    InSim.Send_MTC_MessageToConnection("^9 Can't Send more than 9000!", MSO.UCID, 0);
                                }
                                else if (Connections[GetConnIdx(MSO.UCID)].Cash < Send)
                                {
                                    InSim.Send_MTC_MessageToConnection("^9 Not Enough Money to Send the Transfer", MSO.UCID, 0);
                                }
                                else
                                {
                                    C.Cash += Send;
                                    Conn.Cash -= Send;
                                    MsgAll("^9 " + Conn.PlayerName + " ^7sent ^2$" + Send);
                                    MsgAll("^9 To: " + C.PlayerName + " (" + C.Username + ")");
                                }
                            }
                        }
                        if (SendFound == false)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 Username not Exists or Not Found", MSO.UCID, 0);
                        }
                    }
                    #endregion
                }
                catch
                {
                    InSim.Send_MTC_MessageToConnection("^1»^7 Values to high or Incomplete Command", MSO.UCID, 0);
                }
            }
            else
            {
                if (StrMsg.Length == 1)
                {
                    MsgPly("^9 Sending parameter ^2!send X Y", MSO.UCID);
                    MsgPly("^9 X: Username Y: Send Cash", MSO.UCID);
                }
                else
                {
                    MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                }
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 4;
        }

        [Command("pm", "pm <username> <message>")]
        public void pm(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length > 2)
            {
                if (Connections[GetConnIdx(MSO.UCID)].Username == StrMsg[1] && StrMsg[1].Length > 1)
                {
                    InSim.Send_MTC_MessageToConnection("^9 You can't send PM to yourself!", MSO.UCID, 0);
                }
                else
                {
                    clsConnection Conn = Connections[GetConnIdx(MSO.UCID)];
                    bool PMUserFound = false;
                    foreach (clsConnection C in Connections)
                    {
                        string Message = Msg.Remove(0, C.Username.Length + 5);

                        if (C.Username == StrMsg[1] && StrMsg[1].Length > 1)
                        {
                            PMUserFound = true;

                            InSim.Send_MTC_MessageToConnection("^9 Message Sent To: ^7" + C.PlayerName + " (" + C.Username + ")", MSO.UCID, 0);
                            InSim.Send_MTC_MessageToConnection("^9 Msg: ^7" + Message, MSO.UCID, 0);



                            InSim.Send_MTC_MessageToConnection("^9 PM From: ^7" + Conn.PlayerName + " (" + Conn.Username + ")", C.UniqueID, 0);
                            InSim.Send_MTC_MessageToConnection("^9 Msg: ^7" + Message, C.UniqueID, 0);
                            InSim.Send_MTC_MessageToConnection("^9 To reply use ^2!pm " + Conn.Username + " <message>", C.UniqueID, 0);

                            foreach (clsConnection F in Connections)
                            {
                                if ((F.IsAdmin == 1 && F.IsSuperAdmin == 1) && F.UniqueID != MSO.UCID)
                                {
                                    InSim.Send_MTC_MessageToConnection("^9 PM From: ^7" + Conn.PlayerName + " to " + C.PlayerName, F.UniqueID, 0);
                                    InSim.Send_MTC_MessageToConnection("^9 Msg: ^7" + Message, F.UniqueID, 0);
                                }
                            }
                        }
                    }
                    if (PMUserFound == false)
                    {
                        InSim.Send_MTC_MessageToConnection("^9 Username not found.", MSO.UCID, 0);
                    }
                }
            }
            else
            {
                InSim.Send_MTC_MessageToConnection("^9 Invalid Command.", MSO.UCID, 0);
            }
        }

        [Command("towrequest", "towrequest")]
        public void towreq(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];

            if (Conn.InGame == 0)
            {
                MsgPly("^9 You must be in vehicle before you access this command!", MSO.UCID);
            }
            else
            {
                if (Conn.Location.Contains("Service Station"))
                {
                    MsgPly("^9 You can't call a tow request in Service Station!", MSO.UCID);
                }
                else
                {
                    if (Conn.IsTowTruck == false)
                    {

                        if (Conn.IsBeingTowed == false)
                        {
                            if (Conn.CompCar.Speed / 91 < 5)
                            {
                                if (Conn.CalledRequest == false)
                                {
                                    #region ' get request '
                                    bool Found = false;
                                    foreach (clsConnection i in Connections)
                                    {
                                        if (i.IsTowTruck == true && i.CanBeTowTruck == 1)
                                        {
                                            Found = true;
                                            MsgPly("^9 " + Conn.PlayerName + " ^7called a Request!", i.UniqueID);
                                            MsgPly("^9 Located at ^3" + Conn.Location, i.UniqueID);
                                            MsgPly("^9 To Accept Request ^2!accepttow " + Conn.Username, i.UniqueID);
                                        }
                                    }
                                    if (Found == true)
                                    {
                                        if (Conn.CalledRequest == false)
                                        {
                                            MsgPly("^9 Please wait till your request reached!", MSO.UCID);
                                            Conn.CalledRequest = true;
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^9 There are no Tow Trucks online :(", MSO.UCID);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    MsgPly("^9 You have called a Tow Request Please wait.", MSO.UCID);
                                }
                            }
                            else
                            {
                                MsgPly("^9 You can't call a request whilst your vehicle is running!", MSO.UCID);
                            }
                        }
                        else
                        {
                            MsgPly("^9 Your already being towed!", MSO.UCID);

                        }
                    }
                    else
                    {
                        MsgPly("^9 You can't call a request whilst being duty!", MSO.UCID);
                    }
                }
            }

            Conn.WaitCMD = 4;
        }


        #endregion

        #region ' Player Settings Command '

        [Command("coppanel", "coppanel")]
        public void coppanel(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length == 1)
            {
                if (Conn.CanBeOfficer == 1 || Conn.CanBeCadet == 1)
                {
                    if (Conn.CopPanel == 0)
                    {
                        if (Conn.IsOfficer == true || Conn.IsCadet == true)
                        {
                            MsgPly("^9 Panel Clicks are now Enabled.", MSO.UCID);
                        }
                        else
                        {
                            MsgPly("^9 Panel Clicks are now Enabled.", MSO.UCID);
                            MsgPly("  ^7You need to be duty to look at the Panel!", MSO.UCID);
                        }
                        Conn.CopPanel = 1;
                    }
                    else if (Conn.CopPanel == 1)
                    {
                        if (Conn.IsOfficer == true || Conn.IsCadet == true)
                        {
                            MsgPly("^9 Panel Clicks are now Disabled.", MSO.UCID);
                        }
                        else
                        {
                            MsgPly("^9 Panel Clicks are now Disabled.", MSO.UCID);
                            MsgPly("  ^7You need to be duty to look at the Panel!", MSO.UCID);
                        }
                        Conn.CopPanel = 0;
                    }
                }
                #region ' Not Authorized or Failed '
                else
                {
                    MsgPly("^9 Not Authorized.", MSO.UCID);
                }
                #endregion
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("channel", "channel <Lang>")]
        public void channel(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                bool FoundChannel = false;
                var ToUpper = StrMsg[1].ToUpper();

                #region ' Default English '
                if (ToUpper == "ENG")
                {
                    Conn.ChannelLanguage = 0;
                    MsgPly("^9 Channel switch to Default English.", MSO.UCID);
                    FoundChannel = true;
                }
                #endregion

                #region ' Deutch/German '
                if (ToUpper == "DEU")
                {
                    Conn.ChannelLanguage = 1;
                    MsgPly("^9 Channel switch to Deustch Settings", MSO.UCID);
                    FoundChannel = true;
                }
                #endregion

                #region ' Czech '
                if (ToUpper == "CZE")
                {
                    Conn.ChannelLanguage = 2;
                    MsgPly("^9 Channel switched to Czech Settings", MSO.UCID);
                    FoundChannel = true;
                }
                #endregion

                #region ' Estonian '
                if (ToUpper == "EST")
                {
                    Conn.ChannelLanguage = 3;
                    MsgPly("^9 Channel switched to Estonian Settings", MSO.UCID);
                    FoundChannel = true;
                }
                #endregion

                #region ' Netherlands/Dutch '
                if (ToUpper == "NL")
                {
                    Conn.ChannelLanguage = 4;
                    MsgPly("^9 Channel switched to Dutch Settings", MSO.UCID);
                    FoundChannel = true;
                }
                #endregion

                #region ' Hungarian '
                if (ToUpper == "HUN")
                {
                    Conn.ChannelLanguage = 5;
                    MsgPly("^9 Channel switched to Hungarian Settings", MSO.UCID);
                    FoundChannel = true;
                }
                #endregion

                if (FoundChannel == false)
                {
                    MsgPly("^9 Sorry Channel " + StrMsg[1] + " is not yet Made", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^9 Invalid Channel Command. ^2!channel <Language>", MSO.UCID);
            }
        }

        [Command("ch", "ch <msg>")]
        public void ch(string Msg, String[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                string RemoveMsg = Msg.Remove(0, 3);
                bool ChannelCorrect = false;

                #region ' English '
                if (Conn.ChannelLanguage == 0)
                {
                    MsgPly("^9 Please use the Regular chat!", MSO.UCID);
                    ChannelCorrect = true;
                }
                #endregion

                #region ' Localizations '

                foreach (clsConnection Ch in Connections)
                {
                    #region ' Deutschland '

                    if (Ch.ChannelLanguage == 1)
                    {
                        MsgPly(Conn.PlayerName + "^7 [DEU Ch.] :^2" + RemoveMsg, Ch.UniqueID);
                        ChannelCorrect = true;
                    }

                    #endregion

                    #region ' Czech '

                    if (Ch.ChannelLanguage == 2)
                    {
                        MsgPly(Conn.PlayerName + "^7 [CZE Ch.] :^2" + RemoveMsg, Ch.UniqueID);
                        ChannelCorrect = true;
                    }

                    #endregion

                }

                if (ChannelCorrect == false)
                {
                    MsgPly("^9 Your Channel Settings is Incorrect.", MSO.UCID);
                }

                #endregion
            }
            else
            {
                MsgPly("^9 Invalid Channel Command. ^2!ch <Msg>", MSO.UCID);
            }
        }


        #endregion

        #region ' Prices, Buy and Sell Command '

        [Command("prices", "prices")]
        public void prices(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length == 1)
            {
                long Cash = Connections[GetConnIdx(MSO.UCID)].Cash;
                InSim.Send_MTC_MessageToConnection(CruiseName + " ^7Dealership: ", MSO.UCID, 0);
                InSim.Send_MTC_MessageToConnection("^9 Carname - ^1Buy ^7- ^2Sell ^7- ^2Status ^7- ^2Drivable", MSO.UCID, 0);

                // Normal Range
                #region ' XRG '
                if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains("XRG"))
                {
                    if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 199)
                    {
                        InSim.Send_MTC_MessageToConnection("^9 XRG - ^1$" + Dealer.GetCarPrice("XRG") + " ^7- ^2$" + Dealer.GetCarValue("XRG") + " ^7- ^2Owned ^7- X", (MSO.UCID), (MSO.PLID));
                    }
                    else
                    {
                        InSim.Send_MTC_MessageToConnection("^9 XRG - ^1$" + Dealer.GetCarPrice("XRG") + " ^7- ^2$" + Dealer.GetCarValue("XRG") + " ^7- ^2Owned ^7- OK", (MSO.UCID), (MSO.PLID));
                    }
                }
                else
                {
                    if (Connections[GetConnIdx(MSO.UCID)].Cash >= Dealer.GetCarPrice("XRG"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 199)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 XRG - ^1$" + Dealer.GetCarPrice("XRG") + " ^7- ^2$" + Dealer.GetCarValue("XRG") + " ^7- ^2Have Enough ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 XRG - ^1$" + Dealer.GetCarPrice("XRG") + " ^7- ^2$" + Dealer.GetCarValue("XRG") + " ^7- ^2Have Enough ^7- OK", (MSO.UCID), 0);
                        }
                    }
                    if (Connections[GetConnIdx(MSO.UCID)].Cash < Dealer.GetCarPrice("XRG"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 199)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 XRG - ^1$" + Dealer.GetCarPrice("XRG") + " ^7- ^2$" + Dealer.GetCarValue("XRG") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("XRG") - Cash) + " ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 XRG - ^1$" + Dealer.GetCarPrice("XRG") + " ^7- ^2$" + Dealer.GetCarValue("XRG") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("XRG") - Cash) + " ^7- OK", (MSO.UCID), 0);
                        }
                    }
                }
                #endregion

                // Turbo Powered and Engined Range
                #region ' LX4 '
                if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains("LX4"))
                {
                    if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 699)
                    {
                        InSim.Send_MTC_MessageToConnection("^9 LX4 - ^1$" + Dealer.GetCarPrice("LX4") + " ^7- ^2$" + Dealer.GetCarValue("LX4") + " ^7- ^2Owned ^7- X", (MSO.UCID), (MSO.PLID));
                    }
                    else
                    {
                        InSim.Send_MTC_MessageToConnection("^9 LX4 - ^1$" + Dealer.GetCarPrice("LX4") + " ^7- ^2$" + Dealer.GetCarValue("LX4") + " ^7- ^2Owned ^7- OK", (MSO.UCID), (MSO.PLID));
                    }
                }
                else
                {
                    if (Connections[GetConnIdx(MSO.UCID)].Cash >= Dealer.GetCarPrice("LX4"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 699)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 LX4 - ^1$" + Dealer.GetCarPrice("LX4") + " ^7- ^2$" + Dealer.GetCarValue("LX4") + " ^7- ^2Have Enough ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 LX4 - ^1$" + Dealer.GetCarPrice("LX4") + " ^7- ^2$" + Dealer.GetCarValue("LX4") + " ^7- ^2Have Enough ^7- OK", (MSO.UCID), 0);
                        }
                    }
                    if (Connections[GetConnIdx(MSO.UCID)].Cash < Dealer.GetCarPrice("LX4"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 699)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 LX4 - ^1$" + Dealer.GetCarPrice("LX4") + " ^7- ^2$" + Dealer.GetCarValue("LX4") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("LX4") - Cash) + " ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 LX4 - ^1$" + Dealer.GetCarPrice("LX4") + " ^7- ^2$" + Dealer.GetCarValue("LX4") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("LX4") - Cash) + " ^7- OK", (MSO.UCID), 0);
                        }
                    }
                }
                #endregion

                #region ' RB4 '
                if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains("RB4"))
                {
                    if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 699)
                    {
                        InSim.Send_MTC_MessageToConnection("^9 RB4 - ^1$" + Dealer.GetCarPrice("RB4") + " ^7- ^2$" + Dealer.GetCarValue("RB4") + " ^7- ^2Owned ^7- X", (MSO.UCID), (MSO.PLID));
                    }
                    else
                    {
                        InSim.Send_MTC_MessageToConnection("^9 RB4 - ^1$" + Dealer.GetCarPrice("RB4") + " ^7- ^2$" + Dealer.GetCarValue("RB4") + " ^7- ^2Owned ^7- OK", (MSO.UCID), (MSO.PLID));
                    }
                }
                else
                {
                    if (Connections[GetConnIdx(MSO.UCID)].Cash >= Dealer.GetCarPrice("RB4"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 699)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 RB4 - ^1$" + Dealer.GetCarPrice("RB4") + " ^7- ^2$" + Dealer.GetCarValue("RB4") + " ^7- ^2Have Enough ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 RB4 - ^1$" + Dealer.GetCarPrice("RB4") + " ^7- ^2$" + Dealer.GetCarValue("RB4") + " ^7- ^2Have Enough ^7- OK", (MSO.UCID), 0);
                        }
                    }
                    if (Connections[GetConnIdx(MSO.UCID)].Cash < Dealer.GetCarPrice("RB4"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 699)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 RB4 - ^1$" + Dealer.GetCarPrice("RB4") + " ^7- ^2$" + Dealer.GetCarValue("RB4") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("RB4") - Cash) + " ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 RB4 - ^1$" + Dealer.GetCarPrice("RB4") + " ^7- ^2$" + Dealer.GetCarValue("RB4") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("RB4") - Cash) + " ^7- OK", (MSO.UCID), 0);
                        }
                    }
                }
                #endregion

                #region ' FXO '
                if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains("FXO"))
                {
                    if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 699)
                    {
                        InSim.Send_MTC_MessageToConnection("^9 FXO - ^1$" + Dealer.GetCarPrice("FXO") + " ^7- ^2$" + Dealer.GetCarValue("FXO") + " ^7- ^2Owned ^7- X", (MSO.UCID), (MSO.PLID));
                    }
                    else
                    {
                        InSim.Send_MTC_MessageToConnection("^9 FXO - ^1$" + Dealer.GetCarPrice("FXO") + " ^7- ^2$" + Dealer.GetCarValue("FXO") + " ^7- ^2Owned ^7- OK", (MSO.UCID), (MSO.PLID));
                    }
                }
                else
                {
                    if (Connections[GetConnIdx(MSO.UCID)].Cash >= Dealer.GetCarPrice("FXO"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 699)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FXO - ^1$" + Dealer.GetCarPrice("FXO") + " ^7- ^2$" + Dealer.GetCarValue("FXO") + " ^7- ^2Have Enough ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FXO - ^1$" + Dealer.GetCarPrice("FXO") + " ^7- ^2$" + Dealer.GetCarValue("FXO") + " ^7- ^2Have Enough ^7- OK", (MSO.UCID), 0);
                        }
                    }
                    if (Connections[GetConnIdx(MSO.UCID)].Cash < Dealer.GetCarPrice("FXO"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 699)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FXO - ^1$" + Dealer.GetCarPrice("FXO") + " ^7- ^2$" + Dealer.GetCarValue("FXO") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("FXO") - Cash) + " ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FXO - ^1$" + Dealer.GetCarPrice("FXO") + " ^7- ^2$" + Dealer.GetCarValue("FXO") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("FXO") - Cash) + " ^7- OK", (MSO.UCID), 0);
                        }
                    }
                }
                #endregion

                #region ' VWS '
                if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains("VWS"))
                {
                    if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 699)
                    {
                        InSim.Send_MTC_MessageToConnection("^9 VWS - ^1$" + Dealer.GetCarPrice("VWS") + " ^7- ^2$" + Dealer.GetCarValue("VWS") + " ^7- ^2Owned ^7- X", (MSO.UCID), (MSO.PLID));
                    }
                    else
                    {
                        InSim.Send_MTC_MessageToConnection("^9 VWS - ^1$" + Dealer.GetCarPrice("VWS") + " ^7- ^2$" + Dealer.GetCarValue("VWS") + " ^7- ^2Owned ^7- OK", (MSO.UCID), (MSO.PLID));
                    }
                }
                else
                {
                    if (Connections[GetConnIdx(MSO.UCID)].Cash >= Dealer.GetCarPrice("VWS"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 699)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 VWS - ^1$" + Dealer.GetCarPrice("VWS") + " ^7- ^2$" + Dealer.GetCarValue("VWS") + " ^7- ^2Have Enough ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 VWS - ^1$" + Dealer.GetCarPrice("VWS") + " ^7- ^2$" + Dealer.GetCarValue("VWS") + " ^7- ^2Have Enough ^7- OK", (MSO.UCID), 0);
                        }
                    }
                    if (Connections[GetConnIdx(MSO.UCID)].Cash < Dealer.GetCarPrice("VWS"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 699)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 VWS - ^1$" + Dealer.GetCarPrice("VWS") + " ^7- ^2$" + Dealer.GetCarValue("VWS") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("VWS") - Cash) + " ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 VWS - ^1$" + Dealer.GetCarPrice("VWS") + " ^7- ^2$" + Dealer.GetCarValue("VWS") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("VWS") - Cash) + " ^7- OK", (MSO.UCID), 0);
                        }
                    }
                }
                #endregion

                #region ' XRT '
                if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains("XRT"))
                {
                    if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 699)
                    {
                        InSim.Send_MTC_MessageToConnection("^9 XRT - ^1$" + Dealer.GetCarPrice("XRT") + " ^7- ^2$" + Dealer.GetCarValue("XRT") + " ^7- ^2Owned ^7- X", (MSO.UCID), (MSO.PLID));
                    }
                    else
                    {
                        InSim.Send_MTC_MessageToConnection("^9 XRT - ^1$" + Dealer.GetCarPrice("XRT") + " ^7- ^2$" + Dealer.GetCarValue("XRT") + " ^7- ^2Owned ^7- OK", (MSO.UCID), (MSO.PLID));
                    }
                }
                else
                {
                    if (Connections[GetConnIdx(MSO.UCID)].Cash >= Dealer.GetCarPrice("XRT"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 699)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 XRT - ^1$" + Dealer.GetCarPrice("XRT") + " ^7- ^2$" + Dealer.GetCarValue("XRT") + " ^7- ^2Have Enough ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 XRT - ^1$" + Dealer.GetCarPrice("XRT") + " ^7- ^2$" + Dealer.GetCarValue("XRT") + " ^7- ^2Have Enough ^7- OK", (MSO.UCID), 0);
                        }
                    }
                    if (Connections[GetConnIdx(MSO.UCID)].Cash < Dealer.GetCarPrice("XRT"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 699)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 XRT - ^1$" + Dealer.GetCarPrice("XRT") + " ^7- ^2$" + Dealer.GetCarValue("XRT") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("XRT") - Cash) + " ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 XRT - ^1$" + Dealer.GetCarPrice("XRT") + " ^7- ^2$" + Dealer.GetCarValue("XRT") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("XRT") - Cash) + " ^7- OK", (MSO.UCID), 0);
                        }
                    }
                }
                #endregion

                // Road Car Range
                #region ' LX6 '
                if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains("LX6"))
                {
                    if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 899)
                    {
                        InSim.Send_MTC_MessageToConnection("^9 LX6 - ^1$" + Dealer.GetCarPrice("LX6") + " ^7- ^2$" + Dealer.GetCarValue("LX6") + " ^7- ^2Owned ^7- X", (MSO.UCID), (MSO.PLID));
                    }
                    else
                    {
                        InSim.Send_MTC_MessageToConnection("^9 LX6 - ^1$" + Dealer.GetCarPrice("LX6") + " ^7- ^2$" + Dealer.GetCarValue("LX6") + " ^7- ^2Owned ^7- OK", (MSO.UCID), (MSO.PLID));
                    }
                }
                else
                {
                    if (Connections[GetConnIdx(MSO.UCID)].Cash >= Dealer.GetCarPrice("LX6"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 899)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 LX6 - ^1$" + Dealer.GetCarPrice("LX6") + " ^7- ^2$" + Dealer.GetCarValue("LX6") + " ^7- ^2Have Enough ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 LX6 - ^1$" + Dealer.GetCarPrice("LX6") + " ^7- ^2$" + Dealer.GetCarValue("LX6") + " ^7- ^2Have Enough ^7- OK", (MSO.UCID), 0);
                        }
                    }
                    if (Connections[GetConnIdx(MSO.UCID)].Cash < Dealer.GetCarPrice("LX6"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 899)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 LX6 - ^1$" + Dealer.GetCarPrice("LX6") + " ^7- ^2$" + Dealer.GetCarValue("LX6") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("LX6") - Cash) + " ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 LX6 - ^1$" + Dealer.GetCarPrice("LX6") + " ^7- ^2$" + Dealer.GetCarValue("LX6") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("LX6") - Cash) + " ^7- OK", (MSO.UCID), 0);
                        }
                    }
                }
                #endregion

                #region ' RAC '
                if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains("RAC"))
                {
                    if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 899)
                    {
                        InSim.Send_MTC_MessageToConnection("^9 RAC - ^1$" + Dealer.GetCarPrice("RAC") + " ^7- ^2$" + Dealer.GetCarValue("RAC") + " ^7- ^2Owned ^7- X", (MSO.UCID), (MSO.PLID));
                    }
                    else
                    {
                        InSim.Send_MTC_MessageToConnection("^9 RAC - ^1$" + Dealer.GetCarPrice("RAC") + " ^7- ^2$" + Dealer.GetCarValue("RAC") + " ^7- ^2Owned ^7- OK", (MSO.UCID), (MSO.PLID));
                    }
                }
                else
                {
                    if (Connections[GetConnIdx(MSO.UCID)].Cash >= Dealer.GetCarPrice("RAC"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 899)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 RAC - ^1$" + Dealer.GetCarPrice("RAC") + " ^7- ^2$" + Dealer.GetCarValue("RAC") + " ^7- ^2Have Enough ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 RAC - ^1$" + Dealer.GetCarPrice("RAC") + " ^7- ^2$" + Dealer.GetCarValue("RAC") + " ^7- ^2Have Enough ^7- OK", (MSO.UCID), 0);
                        }
                    }
                    if (Connections[GetConnIdx(MSO.UCID)].Cash < Dealer.GetCarPrice("RAC"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 899)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 RAC - ^1$" + Dealer.GetCarPrice("RAC") + " ^7- ^2$" + Dealer.GetCarValue("RAC") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("RAC") - Cash) + " ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 RAC - ^1$" + Dealer.GetCarPrice("RAC") + " ^7- ^2$" + Dealer.GetCarValue("RAC") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("RAC") - Cash) + " ^7- OK", (MSO.UCID), 0);
                        }
                    }
                }
                #endregion

                #region ' FZ5 '
                if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains("FZ5"))
                {
                    if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 899)
                    {
                        InSim.Send_MTC_MessageToConnection("^9 FZ5 - ^1$" + Dealer.GetCarPrice("FZ5") + " ^7- ^2$" + Dealer.GetCarValue("FZ5") + " ^7- ^2Owned ^7- X", (MSO.UCID), (MSO.PLID));
                    }
                    else
                    {
                        InSim.Send_MTC_MessageToConnection("^9 FZ5 - ^1$" + Dealer.GetCarPrice("FZ5") + " ^7- ^2$" + Dealer.GetCarValue("FZ5") + " ^7- ^2Owned ^7- OK", (MSO.UCID), (MSO.PLID));
                    }
                }
                else
                {
                    if (Connections[GetConnIdx(MSO.UCID)].Cash >= Dealer.GetCarPrice("FZ5"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 899)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FZ5 - ^1$" + Dealer.GetCarPrice("FZ5") + " ^7- ^2$" + Dealer.GetCarValue("FZ5") + " ^7- ^2Have Enough ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FZ5 - ^1$" + Dealer.GetCarPrice("FZ5") + " ^7- ^2$" + Dealer.GetCarValue("FZ5") + " ^7- ^2Have Enough ^7- OK", (MSO.UCID), 0);
                        }
                    }
                    if (Connections[GetConnIdx(MSO.UCID)].Cash < Dealer.GetCarPrice("FZ5"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 899)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FZ5 - ^1$" + Dealer.GetCarPrice("FZ5") + " ^7- ^2$" + Dealer.GetCarValue("FZ5") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("FZ5") - Cash) + " ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FZ5 - ^1$" + Dealer.GetCarPrice("FZ5") + " ^7- ^2$" + Dealer.GetCarValue("FZ5") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("FZ5") - Cash) + " ^7- OK", (MSO.UCID), 0);
                        }
                    }
                }
                #endregion

                // First two GTR Range
                #region ' UFR '
                if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains("UFR"))
                {
                    if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 999)
                    {
                        InSim.Send_MTC_MessageToConnection("^9 UFR - ^1$" + Dealer.GetCarPrice("UFR") + " ^7- ^2$" + Dealer.GetCarValue("UFR") + " ^7- ^2Owned ^7- X", (MSO.UCID), (MSO.PLID));
                    }
                    else
                    {
                        InSim.Send_MTC_MessageToConnection("^9 UFR - ^1$" + Dealer.GetCarPrice("UFR") + " ^7- ^2$" + Dealer.GetCarValue("UFR") + " ^7- ^2Owned ^7- OK", (MSO.UCID), (MSO.PLID));
                    }
                }
                else
                {
                    if (Connections[GetConnIdx(MSO.UCID)].Cash >= Dealer.GetCarPrice("UFR"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 999)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 UFR - ^1$" + Dealer.GetCarPrice("UFR") + " ^7- ^2$" + Dealer.GetCarValue("UFR") + " ^7- ^2Have Enough ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 UFR - ^1$" + Dealer.GetCarPrice("UFR") + " ^7- ^2$" + Dealer.GetCarValue("UFR") + " ^7- ^2Have Enough ^7- OK", (MSO.UCID), 0);
                        }
                    }
                    if (Connections[GetConnIdx(MSO.UCID)].Cash < Dealer.GetCarPrice("UFR"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 999)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 UFR - ^1$" + Dealer.GetCarPrice("UFR") + " ^7- ^2$" + Dealer.GetCarValue("UFR") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("UFR") - Cash) + " ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 UFR - ^1$" + Dealer.GetCarPrice("UFR") + " ^7- ^2$" + Dealer.GetCarValue("UFR") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("UFR") - Cash) + " ^7- OK", (MSO.UCID), 0);
                        }
                    }
                }
                #endregion

                #region ' XFR '
                if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains("XFR"))
                {
                    if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 999)
                    {
                        InSim.Send_MTC_MessageToConnection("^9 XFR - ^1$" + Dealer.GetCarPrice("XFR") + " ^7- ^2$" + Dealer.GetCarValue("XFR") + " ^7- ^2Owned ^7- X", (MSO.UCID), (MSO.PLID));
                    }
                    else
                    {
                        InSim.Send_MTC_MessageToConnection("^9 XFR - ^1$" + Dealer.GetCarPrice("XFR") + " ^7- ^2$" + Dealer.GetCarValue("XFR") + " ^7- ^2Owned ^7- OK", (MSO.UCID), (MSO.PLID));
                    }
                }
                else
                {
                    if (Connections[GetConnIdx(MSO.UCID)].Cash >= Dealer.GetCarPrice("XFR"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 999)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 XFR - ^1$" + Dealer.GetCarPrice("XFR") + " ^7- ^2$" + Dealer.GetCarValue("XFR") + " ^7- ^2Have Enough ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 XFR - ^1$" + Dealer.GetCarPrice("XFR") + " ^7- ^2$" + Dealer.GetCarValue("XFR") + " ^7- ^2Have Enough ^7- OK", (MSO.UCID), 0);
                        }
                    }
                    if (Connections[GetConnIdx(MSO.UCID)].Cash < Dealer.GetCarPrice("XFR"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 999)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 XFR - ^1$" + Dealer.GetCarPrice("XFR") + " ^7- ^2$" + Dealer.GetCarValue("XFR") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("XFR") - Cash) + " ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 XFR - ^1$" + Dealer.GetCarPrice("XFR") + " ^7- ^2$" + Dealer.GetCarValue("XFR") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("XFR") - Cash) + " ^7- OK", (MSO.UCID), 0);
                        }
                    }
                }
                #endregion

                // Three Major GTR Range
                #region ' FXR '
                if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains("FXR"))
                {
                    if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 2999)
                    {
                        InSim.Send_MTC_MessageToConnection("^9 FXR - ^1$" + Dealer.GetCarPrice("FXR") + " ^7- ^2$" + Dealer.GetCarValue("FXR") + " ^7- ^2Owned ^7- X", (MSO.UCID), (MSO.PLID));
                    }
                    else
                    {
                        InSim.Send_MTC_MessageToConnection("^9 FXR - ^1$" + Dealer.GetCarPrice("FXR") + " ^7- ^2$" + Dealer.GetCarValue("FXR") + " ^7- ^2Owned ^7- OK", (MSO.UCID), (MSO.PLID));
                    }
                }
                else
                {
                    if (Connections[GetConnIdx(MSO.UCID)].Cash >= Dealer.GetCarPrice("FXR"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 2999)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FXR - ^1$" + Dealer.GetCarPrice("FXR") + " ^7- ^2$" + Dealer.GetCarValue("FXR") + " ^7- ^2Have Enough ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FXR - ^1$" + Dealer.GetCarPrice("FXR") + " ^7- ^2$" + Dealer.GetCarValue("FXR") + " ^7- ^2Have Enough ^7- OK", (MSO.UCID), 0);
                        }
                    }
                    if (Connections[GetConnIdx(MSO.UCID)].Cash < Dealer.GetCarPrice("FXR"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 2999)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FXR - ^1$" + Dealer.GetCarPrice("FXR") + " ^7- ^2$" + Dealer.GetCarValue("FXR") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("FXR") - Cash) + " ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FXR - ^1$" + Dealer.GetCarPrice("FXR") + " ^7- ^2$" + Dealer.GetCarValue("FXR") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("FXR") - Cash) + " ^7- OK", (MSO.UCID), 0);
                        }
                    }
                }
                #endregion

                #region ' XRR '
                if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains("XRR"))
                {
                    if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 2999)
                    {
                        InSim.Send_MTC_MessageToConnection("^9 XRR - ^1$" + Dealer.GetCarPrice("XRR") + " ^7- ^2$" + Dealer.GetCarValue("XRR") + " ^7- ^2Owned ^7- X", (MSO.UCID), (MSO.PLID));
                    }
                    else
                    {
                        InSim.Send_MTC_MessageToConnection("^9 XRR - ^1$" + Dealer.GetCarPrice("XRR") + " ^7- ^2$" + Dealer.GetCarValue("XRR") + " ^7- ^2Owned ^7- OK", (MSO.UCID), (MSO.PLID));
                    }
                }
                else
                {
                    if (Connections[GetConnIdx(MSO.UCID)].Cash >= Dealer.GetCarPrice("XRR"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 2999)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 XRR - ^1$" + Dealer.GetCarPrice("XRR") + " ^7- ^2$" + Dealer.GetCarValue("XRR") + " ^7- ^2Have Enough ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 XRR - ^1$" + Dealer.GetCarPrice("XRR") + " ^7- ^2$" + Dealer.GetCarValue("XRR") + " ^7- ^2Have Enough ^7- OK", (MSO.UCID), 0);
                        }
                    }
                    if (Connections[GetConnIdx(MSO.UCID)].Cash < Dealer.GetCarPrice("XRR"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 2999)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 XRR - ^1$" + Dealer.GetCarPrice("XRR") + " ^7- ^2$" + Dealer.GetCarValue("XRR") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("XRR") - Cash) + " ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 XRR - ^1$" + Dealer.GetCarPrice("XRR") + " ^7- ^2$" + Dealer.GetCarValue("XRR") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("XRR") - Cash) + " ^7- OK", (MSO.UCID), 0);
                        }
                    }
                }
                #endregion

                #region ' FZR '
                if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains("FZR"))
                {
                    if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 2999)
                    {
                        InSim.Send_MTC_MessageToConnection("^9 FZR - ^1$" + Dealer.GetCarPrice("FZR") + " ^7- ^2$" + Dealer.GetCarValue("FZR") + " ^7- ^2Owned ^7- X", (MSO.UCID), (MSO.PLID));
                    }
                    else
                    {
                        InSim.Send_MTC_MessageToConnection("^9 FZR - ^1$" + Dealer.GetCarPrice("FZR") + " ^7- ^2$" + Dealer.GetCarValue("FZR") + " ^7- ^2Owned ^7- OK", (MSO.UCID), (MSO.PLID));
                    }
                }
                else
                {
                    if (Connections[GetConnIdx(MSO.UCID)].Cash >= Dealer.GetCarPrice("FZR"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 2999)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FZR - ^1$" + Dealer.GetCarPrice("FZR") + " ^7- ^2$" + Dealer.GetCarValue("FZR") + " ^7- ^2Have Enough ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FZR - ^1$" + Dealer.GetCarPrice("FZR") + " ^7- ^2$" + Dealer.GetCarValue("FZR") + " ^7- ^2Have Enough ^7- OK", (MSO.UCID), 0);
                        }
                    }
                    if (Connections[GetConnIdx(MSO.UCID)].Cash < Dealer.GetCarPrice("FZR"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 2999)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FZR - ^1$" + Dealer.GetCarPrice("FZR") + " ^7- ^2$" + Dealer.GetCarValue("FZR") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("FZR") - Cash) + " ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FZR - ^1$" + Dealer.GetCarPrice("FZR") + " ^7- ^2$" + Dealer.GetCarValue("FZR") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("FZR") - Cash) + " ^7- OK", (MSO.UCID), 0);
                        }
                    }
                }
                #endregion

                // One Seaters Range
                #region ' MRT '
                if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains("MRT"))
                {
                    if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 2999)
                    {
                        InSim.Send_MTC_MessageToConnection("^9 MRT - ^1$" + Dealer.GetCarPrice("MRT") + " ^7- ^2$" + Dealer.GetCarValue("MRT") + " ^7- ^2Owned ^7- X", (MSO.UCID), (MSO.PLID));
                    }
                    else
                    {
                        InSim.Send_MTC_MessageToConnection("^9 MRT - ^1$" + Dealer.GetCarPrice("MRT") + " ^7- ^2$" + Dealer.GetCarValue("MRT") + " ^7- ^2Owned ^7- OK", (MSO.UCID), (MSO.PLID));
                    }
                }
                else
                {
                    if (Connections[GetConnIdx(MSO.UCID)].Cash >= Dealer.GetCarPrice("MRT"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 2999)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 MRT - ^1$" + Dealer.GetCarPrice("MRT") + " ^7- ^2$" + Dealer.GetCarValue("MRT") + " ^7- ^2Have Enough ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 MRT - ^1$" + Dealer.GetCarPrice("MRT") + " ^7- ^2$" + Dealer.GetCarValue("MRT") + " ^7- ^2Have Enough ^7- OK", (MSO.UCID), 0);
                        }
                    }
                    if (Connections[GetConnIdx(MSO.UCID)].Cash < Dealer.GetCarPrice("MRT"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 2999)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 MRT - ^1$" + Dealer.GetCarPrice("MRT") + " ^7- ^2$" + Dealer.GetCarValue("MRT") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("MRT") - Cash) + " ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 MRT - ^1$" + Dealer.GetCarPrice("MRT") + " ^7- ^2$" + Dealer.GetCarValue("MRT") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("MRT") - Cash) + " ^7- OK", (MSO.UCID), 0);
                        }
                    }
                }
                #endregion

                #region ' FBM '
                if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains("FBM"))
                {
                    if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 2999)
                    {
                        InSim.Send_MTC_MessageToConnection("^9 FBM - ^1$" + Dealer.GetCarPrice("FBM") + " ^7- ^2$" + Dealer.GetCarValue("FBM") + " ^7- ^2Owned ^7- X", (MSO.UCID), (MSO.PLID));
                    }
                    else
                    {
                        InSim.Send_MTC_MessageToConnection("^9 FBM - ^1$" + Dealer.GetCarPrice("FBM") + " ^7- ^2$" + Dealer.GetCarValue("FBM") + " ^7- ^2Owned ^7- OK", (MSO.UCID), (MSO.PLID));
                    }
                }
                else
                {
                    if (Connections[GetConnIdx(MSO.UCID)].Cash >= Dealer.GetCarPrice("FBM"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 2999)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FBM - ^1$" + Dealer.GetCarPrice("FBM") + " ^7- ^2$" + Dealer.GetCarValue("FBM") + " ^7- ^2Have Enough ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FBM - ^1$" + Dealer.GetCarPrice("FBM") + " ^7- ^2$" + Dealer.GetCarValue("FBM") + " ^7- ^2Have Enough ^7- OK", (MSO.UCID), 0);
                        }
                    }
                    if (Connections[GetConnIdx(MSO.UCID)].Cash < Dealer.GetCarPrice("FBM"))
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].TotalDistance / 1000 <= 2999)
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FBM - ^1$" + Dealer.GetCarPrice("FBM") + " ^7- ^2$" + Dealer.GetCarValue("FBM") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("FBM") - Cash) + " ^7- X", (MSO.UCID), 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^9 FBM - ^1$" + Dealer.GetCarPrice("FBM") + " ^7- ^2$" + Dealer.GetCarValue("FBM") + " ^7- ^2Need ^2$" + (Dealer.GetCarPrice("FBM") - Cash) + " ^7- OK", (MSO.UCID), 0);
                        }
                    }
                }
                #endregion
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
        }

        [Command("buy", "buy")]
        public void buy(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (Conn.InHouse1 == false && Conn.InHouse2 == false && Conn.InHouse3 == false && Conn.InSchool == false && Conn.InShop == false && Conn.InStore == false && Conn.InBank == false)
            {


                #region ' Buy Vehicle '
                if (StrMsg.Length == 2)
                {

                    #region ' Check if Exist '
                    if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains(StrMsg[1].ToUpper()))
                    {
                        if (StrMsg[1].ToUpper() == "UF1")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| UF1000 (UF1) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "XFG")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| XF GTi (XFG) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "XRG")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| XR GTi (XRG) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "LX4")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| LX4 (LX4) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "LX6")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| LX6 (LX6) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "RB4")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| RB GT Turbo (RB4) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "FXO")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| FX GT Turbo (FXO) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "VWS")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| Volkswagen Scirroco (VWS) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "XRT")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| XR GT Turbo (XRT) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "RAC")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| Raceabout (RAC) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "FZ5")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| FZ50 (FZ5) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "UFR")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| UF GTR (UFR) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "XFR")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| XF GTR (XFR) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "FXR")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| FX GTR (FXR) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "XRR")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| XR GTR (XRR) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "FZR")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| FZ GTR (FZR) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "MRT")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| McGill Racing Kart (MRT) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "FOX")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| Formula XR (FOX) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "FBM")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| Formula BMW FB02 (FBM) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "FO8")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| Formula V8 (FO8) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "BF1")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| BMW Sauber 1.06 (BF1) ^7is already exist on the Garage", MSO.UCID, 0);
                        }
                    }
                    #endregion

                    #region ' Check if doesn't Exist '
                    else if (Dealer.GetCarPrice(StrMsg[1]) == 0)
                    {
                        if (StrMsg[1].ToUpper() == "UF1")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| UF1000 (UF1) ^7is not available in Dealership", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "XFG")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| XF GTi (XFG) ^7is not available in Dealership", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "FO8")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| Formula V8 (FO8) ^7is not available in Dealership", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "FOX")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| Formula XR (FOX) ^7is not available in Dealership", MSO.UCID, 0);
                        }
                        else if (StrMsg[1].ToUpper() == "BF1")
                        {
                            InSim.Send_MTC_MessageToConnection("^4| BMW Sauber F1.06 (BF1) ^7is not available in Dealership", MSO.UCID, 0);
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^4| " + StrMsg[1].ToUpper() + " ^7does not exist on Dealership", MSO.UCID, 0);
                        }
                    }
                    #endregion

                    #region ' Check if Insuffient Cash '
                    else if (Connections[GetConnIdx(MSO.UCID)].Cash <= Dealer.GetCarPrice(StrMsg[1]))
                    {
                        if (StrMsg[1].ToUpper() == "XRG")
                        {
                            InSim.Send_MTC_MessageToConnection("^9 Not Enough Cash for ^1XR GTi (XRG)", MSO.UCID, 0);
                            if (Connections[GetConnIdx(MSO.UCID)].Cash <= Dealer.GetCarPrice(StrMsg[1]))
                            {
                                InSim.Send_MTC_MessageToConnection("^9 Price: ^1$" + Dealer.GetCarPrice(StrMsg[1]) + " ^7You need: ^2$" + (9000 - Connections[GetConnIdx(MSO.UCID)].Cash), MSO.UCID, 0);
                            }

                        }
                        else if (StrMsg[1].ToUpper() == "LX4")
                        {
                            InSim.Send_MTC_MessageToConnection("^9 Not Enough Cash for ^1LX4 (LX4)", MSO.UCID, 0);
                            if (Connections[GetConnIdx(MSO.UCID)].Cash <= Dealer.GetCarPrice(StrMsg[1]))
                            {
                                InSim.Send_MTC_MessageToConnection("^9 Price: ^1$" + Dealer.GetCarPrice(StrMsg[1]) + " ^7You need: ^2$" + (23000 - Connections[GetConnIdx(MSO.UCID)].Cash), MSO.UCID, 0);
                            }

                        }
                        else if (StrMsg[1].ToUpper() == "LX6")
                        {
                            InSim.Send_MTC_MessageToConnection("^9 Not Enough Cash for ^1LX6 (LX6)", MSO.UCID, 0);
                            if (Connections[GetConnIdx(MSO.UCID)].Cash <= Dealer.GetCarPrice(StrMsg[1]))
                            {
                                InSim.Send_MTC_MessageToConnection("^9 Price: ^1$" + Dealer.GetCarPrice(StrMsg[1]) + " ^7You need: ^2$" + (38000 - Connections[GetConnIdx(MSO.UCID)].Cash), MSO.UCID, 0);
                            }

                        }
                        else if (StrMsg[1].ToUpper() == "RB4")
                        {
                            InSim.Send_MTC_MessageToConnection("^9 Not Enough Cash for ^1RB GT Turbo (RB4)", MSO.UCID, 0);
                            if (Connections[GetConnIdx(MSO.UCID)].Cash <= Dealer.GetCarPrice(StrMsg[1]))
                            {
                                InSim.Send_MTC_MessageToConnection("^9 Price: ^1$" + Dealer.GetCarPrice(StrMsg[1]) + " ^7You need: ^2$" + (18500 - Connections[GetConnIdx(MSO.UCID)].Cash), MSO.UCID, 0);
                            }

                        }
                        else if (StrMsg[1].ToUpper() == "FXO")
                        {
                            InSim.Send_MTC_MessageToConnection("^9 Not Enough Cash for ^1FX GT Turbo (FXO)", MSO.UCID, 0);
                            if (Connections[GetConnIdx(MSO.UCID)].Cash <= Dealer.GetCarPrice(StrMsg[1]))
                            {
                                InSim.Send_MTC_MessageToConnection("^9 Price: ^1$" + Dealer.GetCarPrice(StrMsg[1]) + " ^7You need: ^2$" + (25000 - Connections[GetConnIdx(MSO.UCID)].Cash), MSO.UCID, 0);
                            }

                        }
                        else if (StrMsg[1].ToUpper() == "VWS")
                        {
                            InSim.Send_MTC_MessageToConnection("^9 Not Enough Cash for ^1Volkswagen Scirroco (VWS)", MSO.UCID, 0);
                            if (Connections[GetConnIdx(MSO.UCID)].Cash <= Dealer.GetCarPrice(StrMsg[1]))
                            {
                                InSim.Send_MTC_MessageToConnection("^9 Price: ^1$" + Dealer.GetCarPrice(StrMsg[1]) + " ^7You need: ^2$" + (27000 - Connections[GetConnIdx(MSO.UCID)].Cash), MSO.UCID, 0);
                            }

                        }
                        else if (StrMsg[1].ToUpper() == "XRT")
                        {
                            InSim.Send_MTC_MessageToConnection("^9 Not Enough Cash for ^1XR GT Turbo (XRT)", MSO.UCID, 0);
                            if (Connections[GetConnIdx(MSO.UCID)].Cash <= Dealer.GetCarPrice(StrMsg[1]))
                            {
                                InSim.Send_MTC_MessageToConnection("^9 Price: ^1$" + Dealer.GetCarPrice(StrMsg[1]) + " ^7You need: ^2$" + (32000 - Connections[GetConnIdx(MSO.UCID)].Cash), MSO.UCID, 0);
                            }

                        }
                        else if (StrMsg[1].ToUpper() == "RAC")
                        {
                            InSim.Send_MTC_MessageToConnection("^9 Not Enough Cash for ^1Raceabout (RAC)", MSO.UCID, 0);
                            if (Connections[GetConnIdx(MSO.UCID)].Cash <= Dealer.GetCarPrice(StrMsg[1]))
                            {
                                InSim.Send_MTC_MessageToConnection("^9 Price: ^1$" + Dealer.GetCarPrice(StrMsg[1]) + " ^7You need: ^2$" + (46000 - Connections[GetConnIdx(MSO.UCID)].Cash), MSO.UCID, 0);
                            }

                        }
                        else if (StrMsg[1].ToUpper() == "FZ5")
                        {
                            InSim.Send_MTC_MessageToConnection("^9 Not Enough Cash for ^1FZ50 GT (FZ5)", MSO.UCID, 0);
                            if (Connections[GetConnIdx(MSO.UCID)].Cash <= Dealer.GetCarPrice(StrMsg[1]))
                            {
                                InSim.Send_MTC_MessageToConnection("^9 Price: ^1$" + Dealer.GetCarPrice(StrMsg[1]) + " ^7You need: ^2$" + (60000 - Connections[GetConnIdx(MSO.UCID)].Cash), MSO.UCID, 0);
                            }

                        }
                        else if (StrMsg[1].ToUpper() == "UFR")
                        {
                            InSim.Send_MTC_MessageToConnection("^9 Not Enough Cash for ^1UF GTR (UFR)", MSO.UCID, 0);
                            if (Connections[GetConnIdx(MSO.UCID)].Cash <= Dealer.GetCarPrice(StrMsg[1]))
                            {
                                InSim.Send_MTC_MessageToConnection("^9 Price: ^1$" + Dealer.GetCarPrice(StrMsg[1]) + " ^7You need: ^2$" + (130000 - Connections[GetConnIdx(MSO.UCID)].Cash), MSO.UCID, 0);
                            }

                        }
                        else if (StrMsg[1].ToUpper() == "XFR")
                        {
                            InSim.Send_MTC_MessageToConnection("^9 Not Enough Cash for ^1XF GTR (XFR)", MSO.UCID, 0);
                            if (Connections[GetConnIdx(MSO.UCID)].Cash <= Dealer.GetCarPrice(StrMsg[1]))
                            {
                                InSim.Send_MTC_MessageToConnection("^9 Price: ^1$" + Dealer.GetCarPrice(StrMsg[1]) + " ^7You need: ^2$" + (150000 - Connections[GetConnIdx(MSO.UCID)].Cash), MSO.UCID, 0);
                            }

                        }
                        else if (StrMsg[1].ToUpper() == "FXR")
                        {
                            InSim.Send_MTC_MessageToConnection("^9 Not Enough Cash for ^1FX GTR (FXR)", MSO.UCID, 0);
                            if (Connections[GetConnIdx(MSO.UCID)].Cash <= Dealer.GetCarPrice(StrMsg[1]))
                            {
                                InSim.Send_MTC_MessageToConnection("^9 Price: ^1$" + Dealer.GetCarPrice(StrMsg[1]) + " ^7You need: ^2$" + (350000 - Connections[GetConnIdx(MSO.UCID)].Cash), MSO.UCID, 0);
                            }

                        }
                        else if (StrMsg[1].ToUpper() == "XRR")
                        {
                            InSim.Send_MTC_MessageToConnection("^9 Not Enough Cash for ^1XR GTR (XRR)", MSO.UCID, 0);
                            if (Connections[GetConnIdx(MSO.UCID)].Cash <= Dealer.GetCarPrice(StrMsg[1]))
                            {
                                InSim.Send_MTC_MessageToConnection("^9 Price: ^1$" + Dealer.GetCarPrice(StrMsg[1]) + " ^7You need: ^2$" + (400000 - Connections[GetConnIdx(MSO.UCID)].Cash), MSO.UCID, 0);
                            }

                        }
                        else if (StrMsg[1].ToUpper() == "FZR")
                        {
                            InSim.Send_MTC_MessageToConnection("^9 Not Enough Cash for ^1FZ GTR (FZR)", MSO.UCID, 0);
                            if (Connections[GetConnIdx(MSO.UCID)].Cash <= Dealer.GetCarPrice(StrMsg[1]))
                            {
                                InSim.Send_MTC_MessageToConnection("^9 Price: ^1$" + Dealer.GetCarPrice(StrMsg[1]) + " ^7You need: ^2$" + (500000 - Connections[GetConnIdx(MSO.UCID)].Cash), MSO.UCID, 0);
                            }

                        }
                        else if (StrMsg[1].ToUpper() == "MRT")
                        {
                            InSim.Send_MTC_MessageToConnection("^9 Not Enough Cash for ^1McGill Racing Kart (MRT)", MSO.UCID, 0);
                            if (Connections[GetConnIdx(MSO.UCID)].Cash <= Dealer.GetCarPrice(StrMsg[1]))
                            {
                                InSim.Send_MTC_MessageToConnection("^9 Price: ^1$" + Dealer.GetCarPrice(StrMsg[1]) + " ^7You need: ^2$" + (85000 - Connections[GetConnIdx(MSO.UCID)].Cash), MSO.UCID, 0);
                            }

                        }
                        else if (StrMsg[1].ToUpper() == "FBM")
                        {
                            InSim.Send_MTC_MessageToConnection("^9 Not Enough Cash for ^1Formula BMW FB02 (FBM)", MSO.UCID, 0);
                            if (Connections[GetConnIdx(MSO.UCID)].Cash <= Dealer.GetCarPrice(StrMsg[1]))
                            {
                                InSim.Send_MTC_MessageToConnection("^9 Price: ^1$" + Dealer.GetCarPrice(StrMsg[1]) + " ^7You need: ^2$" + (Dealer.GetCarPrice(StrMsg[1]) - Connections[GetConnIdx(MSO.UCID)].Cash), MSO.UCID, 0);
                            }
                        }
                    }
                    #endregion

                    #region ' if Cash is flow '
                    else if (Connections[GetConnIdx(MSO.UCID)].Cash >= Dealer.GetCarPrice(StrMsg[1]))
                    {
                        string Cars = Connections[GetConnIdx(MSO.UCID)].Cars;
                        switch (StrMsg[1].ToUpper())
                        {
                            case "XRG":

                                Cars = Cars + " " + "XRG";
                                Connections[GetConnIdx(MSO.UCID)].Cars = Cars;
                                Connections[GetConnIdx(MSO.UCID)].Cash -= Dealer.GetCarPrice("XRG");
                                MsgAll("^9 " + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7bought a ^1XRG");
                                InSim.Send_MTC_MessageToConnection("^9 You have bought ^1XR GTi (XRG) ^7in Garage", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 Price Tag: ^1$" + Dealer.GetCarPrice("XRG") + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash + " ^7left", MSO.UCID, 0);

                                break;

                            case "LX4":

                                Cars = Cars + " " + "LX4";
                                Connections[GetConnIdx(MSO.UCID)].Cars = Cars;
                                Connections[GetConnIdx(MSO.UCID)].Cash -= Dealer.GetCarPrice("LX4");
                                MsgAll("^9 " + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7bought a ^1LX4");
                                InSim.Send_MTC_MessageToConnection("^9 You have bought ^1LX4 (LX4) ^7in Garage", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 Price Tag: ^1$" + Dealer.GetCarPrice("LX4") + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash + " ^7left", MSO.UCID, 0);

                                break;

                            case "LX6":

                                Cars = Cars + " " + "LX6";
                                Connections[GetConnIdx(MSO.UCID)].Cars = Cars;
                                Connections[GetConnIdx(MSO.UCID)].Cash -= Dealer.GetCarPrice("LX6");
                                MsgAll("^9 " + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7bought a ^1LX6");
                                InSim.Send_MTC_MessageToConnection("^9 You have bought ^1LX6 (LX6) ^7in Garage", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 Price Tag: ^1$" + Dealer.GetCarPrice("LX6") + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash + " ^7left", MSO.UCID, 0);

                                break;

                            case "RB4":

                                Cars = Cars + " " + "RB4";
                                Connections[GetConnIdx(MSO.UCID)].Cars = Cars;
                                Connections[GetConnIdx(MSO.UCID)].Cash -= Dealer.GetCarPrice("RB4");
                                MsgAll("^9 " + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7bought a ^1RB4");
                                InSim.Send_MTC_MessageToConnection("^9 You have bought ^1RB GT Turbo (RB4) ^7in Garage", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 Price Tag: ^1$" + Dealer.GetCarPrice("RB4") + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash + " ^7left", MSO.UCID, 0);

                                break;

                            case "FXO":

                                Cars = Cars + " " + "FXO";
                                Connections[GetConnIdx(MSO.UCID)].Cars = Cars;
                                Connections[GetConnIdx(MSO.UCID)].Cash -= Dealer.GetCarPrice("FXO");
                                MsgAll("^9 " + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7bought a ^1FXO");
                                InSim.Send_MTC_MessageToConnection("^9 You have bought ^1FX GT Turbo (FXO) ^7in Garage", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 Price Tag: ^1$" + Dealer.GetCarPrice("FXO") + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash + " ^7left", MSO.UCID, 0);

                                break;

                            case "VWS":

                                Cars = Cars + " " + "VWS";
                                Connections[GetConnIdx(MSO.UCID)].Cars = Cars;
                                Connections[GetConnIdx(MSO.UCID)].Cash -= Dealer.GetCarPrice("VWS");
                                MsgAll("^9 " + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7bought a ^1VWS");
                                InSim.Send_MTC_MessageToConnection("^9 You have bought ^1Volkswagen Scirocco (VWS) ^7in Garage", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 Price Tag: ^1$" + Dealer.GetCarPrice("VWS") + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash + " ^7left", MSO.UCID, 0);

                                break;

                            case "XRT":

                                Cars = Cars + " " + "XRT";
                                Connections[GetConnIdx(MSO.UCID)].Cars = Cars;
                                Connections[GetConnIdx(MSO.UCID)].Cash -= Dealer.GetCarPrice("XRT");
                                MsgAll("^9 " + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7bought a ^1XRT");
                                InSim.Send_MTC_MessageToConnection("^9 You have bought ^1XR GT Turbo (XRT) ^7in Garage", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 Price Tag: ^1$" + Dealer.GetCarPrice("XRT") + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash + " ^7left", MSO.UCID, 0);

                                break;

                            case "RAC":

                                Cars = Cars + " " + "RAC";
                                Connections[GetConnIdx(MSO.UCID)].Cars = Cars;
                                Connections[GetConnIdx(MSO.UCID)].Cash -= Dealer.GetCarPrice("RAC");
                                MsgAll("^9 " + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7bought a ^1RAC");
                                InSim.Send_MTC_MessageToConnection("^9 You have bought ^1Raceabout (RAC) ^7in Garage", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 Price Tag: ^1$" + Dealer.GetCarPrice("RAC") + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash + " ^7left", MSO.UCID, 0);

                                break;

                            case "FZ5":

                                Cars = Cars + " " + "FZ5";
                                Connections[GetConnIdx(MSO.UCID)].Cars = Cars;
                                Connections[GetConnIdx(MSO.UCID)].Cash -= Dealer.GetCarPrice("FZ5");
                                MsgAll("^9 " + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7bought a ^1FZ5");
                                InSim.Send_MTC_MessageToConnection("^9 You have bought ^1FZ50 GT (FZ5) ^7in Garage", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 Price Tag: ^1$" + Dealer.GetCarPrice("LX6") + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash + " ^7left", MSO.UCID, 0);

                                break;

                            case "UFR":

                                Cars = Cars + " " + "UFR";
                                Connections[GetConnIdx(MSO.UCID)].Cars = Cars;
                                Connections[GetConnIdx(MSO.UCID)].Cash -= Dealer.GetCarPrice("UFR");
                                MsgAll("^9 " + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7bought a ^1UFR");
                                InSim.Send_MTC_MessageToConnection("^9 You have bought ^1UF GTR (UFR) ^7in Garage", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 Price Tag: ^1$" + Dealer.GetCarPrice("UFR") + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash + " ^7left", MSO.UCID, 0);

                                break;

                            case "XFR":

                                Cars = Cars + " " + "XFR";
                                Connections[GetConnIdx(MSO.UCID)].Cars = Cars;
                                Connections[GetConnIdx(MSO.UCID)].Cash -= Dealer.GetCarPrice("XFR");
                                MsgAll("^9 " + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7bought a ^1XFR");
                                InSim.Send_MTC_MessageToConnection("^9 You have bought ^1XF GTR (XFR) ^7in Garage", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 Price Tag: ^1$" + Dealer.GetCarPrice("XFR") + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash + " ^7left", MSO.UCID, 0);

                                break;

                            case "FXR":

                                Cars = Cars + " " + "FXR";
                                Connections[GetConnIdx(MSO.UCID)].Cars = Cars;
                                Connections[GetConnIdx(MSO.UCID)].Cash -= Dealer.GetCarPrice("FXR");
                                MsgAll("^9 " + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7bought a ^1FXR");
                                InSim.Send_MTC_MessageToConnection("^9 You have bought ^1FX GTR (FXR) ^7in Garage", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 Price Tag: ^1$" + Dealer.GetCarPrice("FXR") + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash + " ^7left", MSO.UCID, 0);

                                break;

                            case "XRR":

                                Cars = Cars + " " + "XRR";
                                Connections[GetConnIdx(MSO.UCID)].Cars = Cars;
                                Connections[GetConnIdx(MSO.UCID)].Cash -= Dealer.GetCarPrice("XRR");
                                MsgAll("^9 " + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7bought a ^1XRR");
                                InSim.Send_MTC_MessageToConnection("^9 You have bought ^1XR GTR (XRR) ^7in Garage", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 Price Tag: ^1$" + Dealer.GetCarPrice("XRR") + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash + " ^7left", MSO.UCID, 0);


                                break;

                            case "FZR":

                                Cars = Cars + " " + "FZR";
                                Connections[GetConnIdx(MSO.UCID)].Cars = Cars;
                                Connections[GetConnIdx(MSO.UCID)].Cash -= Dealer.GetCarPrice("FZR");
                                MsgAll("^9 " + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7bought a ^1FZR");
                                InSim.Send_MTC_MessageToConnection("^9 You have bought ^1FZ GTR (FZR) ^7in Garage", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 Price Tag: ^1$" + Dealer.GetCarPrice("FZR") + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash + " ^7left", MSO.UCID, 0);

                                break;

                            case "MRT":

                                Cars = Cars + " " + "MRT";
                                Connections[GetConnIdx(MSO.UCID)].Cars = Cars;
                                Connections[GetConnIdx(MSO.UCID)].Cash -= Dealer.GetCarPrice("MRT");
                                MsgAll("^9 " + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7bought a ^1MRT");
                                InSim.Send_MTC_MessageToConnection("^9 You have bought ^1McGill Racing Kart (MRT) ^7in Garage", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 Price Tag: ^1$" + Dealer.GetCarPrice("MRT") + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash + " ^7left", MSO.UCID, 0);

                                break;


                            case "FBM":

                                Cars = Cars + " " + "FBM";
                                Connections[GetConnIdx(MSO.UCID)].Cars = Cars;
                                Connections[GetConnIdx(MSO.UCID)].Cash -= Dealer.GetCarPrice("FBM");
                                MsgAll("^9 " + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7bought a ^1FBM");
                                InSim.Send_MTC_MessageToConnection("^9 You have bought ^1Formula BMW FB02 (FBM) ^7in Garage", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 Price Tag: ^1$" + Dealer.GetCarPrice("FBM") + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash + " ^7left", MSO.UCID, 0);

                                break;
                        }
                    }
                    #endregion
                }
                else
                {
                    if (StrMsg.Length == 1)
                    {
                        MsgPly("^9 Buying Parameter ^2!buy X ^7- eg. ^2!buy XRG", MSO.UCID);
                    }
                    else
                    {
                        MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                    }
                }
                #endregion
            }
            else
            {
                bool Found = false;



                #region ' In Store '

                if (Conn.InStore == true)
                {
                    if (StrMsg.Length == 3)
                    {
                        byte Amount = byte.Parse(StrMsg[2]);
                        if (StrMsg[2].Contains("-"))
                        {
                            Found = true;
                            MsgPly("^9 Invalid Input. Don't put minus on values!", MSO.UCID);
                        }
                        else
                        {
                            #region ' Electronic '
                            if (StrMsg[1] == "electronic")
                            {
                                Found = true;
                                if (Amount > 10)
                                {
                                    MsgPly("^9 Cannot buy more than 10 Electronic Items!", MSO.UCID);
                                }
                                else
                                {
                                    if (Conn.Electronics < 10)
                                    {
                                        if (Conn.Cash > 190 * Amount)
                                        {
                                            Conn.Electronics += Amount;
                                            Conn.Cash -= 190 * Amount;
                                            Conn.TotalSale += Amount;
                                            MsgAll("^9 " + Conn.PlayerName + " bought some Electronic for ^1$" + Amount * 190 + "^7!");
                                            MsgPly("^9 Total Electronic: " + Conn.Electronics, MSO.UCID);
                                            MsgPly("^9 To Sell them visit some nearest houses and start trading!", MSO.UCID);

                                            if (Conn.LastRaffle == 0)
                                            {
                                                MsgPly("^9 Buy more items and more chances of winning in the Raffle!", MSO.UCID);

                                                if (Conn.DisplaysOpen == true && Conn.InStore == true && Conn.InGameIntrfc == 0)
                                                {
                                                    InSim.Send_BTN_CreateButton("^7Total Item bought: ^2(" + Conn.TotalSale + ")^7 Raffle for ^1$300", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);
                                                    InSim.Send_BTN_CreateButton("^2Raffle!", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 100, 120, Conn.UniqueID, 2, false);
                                                }
                                            }
                                            if (Conn.DisplaysOpen == true && Conn.InStore == true && Conn.InGameIntrfc == 0)
                                            {
                                                InSim.Send_BTN_CreateButton("^2Buy", "Maximum Buy 10 and you have " + Conn.Electronics, Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 65, 100, 2, 118, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        else
                                        {
                                            MsgPly("^9 Not Enough Cash to complete the transaction.", MSO.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^9 Cannot carry more than 10 items!", MSO.UCID);
                                    }
                                }
                            }
                            #endregion

                            #region ' Furniture '
                            if (StrMsg[1] == "furniture")
                            {
                                Found = true;
                                if (Conn.Furniture < 10)
                                {
                                    if (Conn.Cash > 150 * Amount)
                                    {
                                        Conn.Furniture += Amount;
                                        Conn.Cash -= 150 * Amount;
                                        Conn.TotalSale += Amount;
                                        MsgAll("^9 " + Conn.PlayerName + " bought a Furniture for ^1$" + Amount * 150 + "^7!");

                                        MsgPly("^9 Total Furniture: " + Conn.Furniture, MSO.UCID);
                                        if (Conn.LastRaffle == 0)
                                        {
                                            MsgPly("^9 Buy more items and more chances of winning in the Raffle!", MSO.UCID);
                                            if (Conn.DisplaysOpen == true && Conn.InStore == true && Conn.InGameIntrfc == 0)
                                            {
                                                InSim.Send_BTN_CreateButton("^7Total Item bought: ^2(" + Conn.TotalSale + ")^7 Raffle for ^1$300", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Raffle!", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 100, 120, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        if (Conn.DisplaysOpen == true && Conn.InStore == true && Conn.InGameIntrfc == 0)
                                        {
                                            InSim.Send_BTN_CreateButton("^2Buy", "Maximum Buy 10 and you have " + Conn.Furniture, Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 69, 100, 2, 119, Conn.UniqueID, 2, false);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^9 Not Enough Cash to complete the transaction.", MSO.UCID);
                                    }
                                }
                                else
                                {
                                    MsgPly("^9 Cannot carry more than 10 items!", MSO.UCID);
                                }
                            }
                            #endregion
                        }
                    }
                    else if (StrMsg.Length == 2)
                    {
                        Found = true;
                        #region ' Raffle '

                        if (StrMsg[1] == "raffle")
                        {
                            if (Conn.Cash >= 300)
                            {
                                if (Conn.LastRaffle == 0)
                                {
                                    #region ' Raffle Accept '
                                    if (Conn.TotalSale >= 1)
                                    {
                                        #region ' Total Sale Line '
                                        if (Conn.TotalSale > 16)
                                        {
                                            int prize = new Random().Next(3000, 5000);
                                            Conn.Cash += prize;
                                            MsgAll("^9 " + Conn.PlayerName + " won ^2$" + prize + " ^7from the Raffle!");
                                        }
                                        else if (Conn.TotalSale > 11)
                                        {
                                            int prize = new Random().Next(2500, 3000);
                                            Conn.Cash += prize;
                                            MsgAll("^9 " + Conn.PlayerName + " won ^2$" + prize + " ^7from the Raffle!");
                                        }
                                        else if (Conn.TotalSale > 6)
                                        {
                                            int prize = new Random().Next(1000, 2500);
                                            Conn.Cash += prize;
                                            MsgAll("^9 " + Conn.PlayerName + " won ^2$" + prize + " ^7from the Raffle!");
                                        }
                                        else if (Conn.TotalSale > 3)
                                        {
                                            int prize = new Random().Next(750, 1000);
                                            Conn.Cash += prize;
                                            MsgAll("^9 " + Conn.PlayerName + " won ^2$" + prize + " ^7from the Raffle!");
                                        }
                                        else
                                        {
                                            int prize = new Random().Next(300, 750);
                                            Conn.Cash += prize;
                                            MsgAll("^9 " + Conn.PlayerName + " won ^2$" + prize + " ^7from the Raffle!");
                                        }
                                        #endregion

                                        Conn.LastRaffle = 180;

                                        #region ' Replace Display '
                                        if (Conn.DisplaysOpen == true && Conn.InGameIntrfc == 0)
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
                                            DeleteBTN(120, Conn.UniqueID);
                                        }
                                        #endregion

                                        Conn.Cash -= 300;
                                        Conn.TotalSale = 0;
                                    }
                                    else
                                    {
                                        MsgPly("^9 You need to buy items before you raffle!", MSO.UCID);
                                    }

                                    #endregion
                                }
                                else
                                {
                                    #region ' Time Warning '
                                    if (Conn.LastRaffle > 120)
                                    {
                                        MsgPly("^9 You have to wait ^1Three (3) hours ^7to rejoin the Raffle", MSO.UCID);
                                    }
                                    else if (Conn.LastRaffle > 60)
                                    {
                                        MsgPly("^9 You have to wait ^1Two (2) hours ^7to rejoin the Raffle", MSO.UCID);
                                    }
                                    else
                                    {
                                        MsgPly("^9 You have to wait ^1" + Conn.LastRaffle + " Minutes ^7to rejoin the Raffle", MSO.UCID);
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                MsgPly("^9 Not Enough Cash to join this raffle", MSO.UCID);
                            }
                        }

                        #endregion
                    }
                }

                #endregion

                #region ' In Shop '

                if (Conn.InShop == true)
                {
                    if (StrMsg.Length == 2)
                    {
                        #region ' Chicken '
                        if (StrMsg[1] == "chicken")
                        {
                            Found = true;
                            if (Conn.Cash > 15)
                            {
                                if (Conn.TotalHealth < 89)
                                {
                                    MsgAll("^9 " + Conn.PlayerName + " ate Fried Chicken!");
                                    Conn.Cash -= 15;
                                    Conn.TotalHealth += 10;
                                }
                                else
                                {
                                    MsgPly("^9 Too much health. Can't buy anymore.", MSO.UCID);
                                }
                            }
                            else
                            {
                                MsgPly("^9 Not Enought cash.", MSO.UCID);
                            }
                        }
                        #endregion

                        #region ' Beer '
                        if (StrMsg[1] == "beer")
                        {
                            Found = true;
                            if (Connections[GetConnIdx(MSO.UCID)].Cash > 10)
                            {
                                if (Connections[GetConnIdx(MSO.UCID)].TotalHealth <= 92)
                                {
                                    MsgAll("^9 " + Conn.PlayerName + " drank some Beer!");
                                    Connections[GetConnIdx(MSO.UCID)].Cash -= 10;
                                    Connections[GetConnIdx(MSO.UCID)].TotalHealth += 7;
                                }
                                else
                                {
                                    MsgPly("^9 Too much health. Can't buy anymore.", MSO.UCID);
                                }
                            }
                            else
                            {
                                MsgPly("^9 Not Enought cash.", MSO.UCID);
                            }
                        }
                        #endregion

                        #region ' Donuts '
                        if (StrMsg[1] == "donuts")
                        {
                            Found = true;
                            if (Conn.Cash > 5)
                            {
                                if (Conn.TotalHealth < 94)
                                {
                                    MsgAll("^9 " + Conn.PlayerName + " bite some Donuts!");
                                    Conn.Cash -= 5;
                                    Conn.TotalHealth += 5;
                                }
                                else
                                {
                                    MsgPly("^9 Too much health. Can't buy anymore.", MSO.UCID);
                                }
                            }
                            else
                            {
                                MsgPly("^9 Not Enought cash.", MSO.UCID);
                            }
                        }
                        #endregion
                    }
                }

                #endregion

                #region ' In KinderGarten '

                if (Conn.InSchool == true)
                {
                    if (StrMsg.Length == 2)
                    {
                        #region ' Cake! '
                        if (StrMsg[1] == "cake")
                        {
                            Found = true;
                            if (Conn.Cash > 15)
                            {
                                if (Conn.TotalHealth < 89)
                                {
                                    MsgAll("^9 " + Conn.PlayerName + " eat some Cake!");
                                    Conn.Cash -= 15;
                                    Conn.TotalHealth += 10;
                                }
                                else
                                {
                                    MsgPly("^9 Too much health. Can't buy anymore.", MSO.UCID);
                                }
                            }
                            else
                            {
                                MsgPly("^9 Not Enought cash.", MSO.UCID);
                            }

                        }
                        #endregion

                        #region ' Lemonade! '
                        if (StrMsg[1] == "lemonade")
                        {
                            if (Conn.Cash > 10)
                            {
                                if (Conn.TotalHealth < 92)
                                {
                                    MsgAll("^9 " + Conn.PlayerName + " drank a Lemonade!");
                                    Conn.Cash -= 10;
                                    Conn.TotalHealth += 7;
                                }
                                else
                                {
                                    MsgPly("^9 Too much health. Can't buy anymore.", MSO.UCID);
                                }
                            }
                            else
                            {
                                MsgPly("^9 Not Enought cash.", MSO.UCID);
                            }
                        }
                        #endregion
                    }
                    else if (StrMsg.Length == 3)
                    {
                        #region ' Lottery '
                        byte LottoPicked = byte.Parse(StrMsg[2]);
                        if (StrMsg[2].Contains("-"))
                        {
                            Found = true;
                            MsgPly("^9 Invalid Input. Don't put minus on values!", MSO.UCID);
                        }
                        else
                        {
                            if (StrMsg[1] == "lotto")
                            {
                                Found = true;
                                var BTT = MSO;
                                #region ' Accept Lottery '
                                if (Conn.Cash > 100)
                                {
                                    if (Conn.LastLotto == 0)
                                    {
                                        if (LottoPicked > 10)
                                        {
                                            MsgPly("^9 Can't pick more than 10 numbers!", BTT.UCID);
                                        }
                                        else if (LottoPicked == 0)
                                        {
                                            MsgPly("^9 Can't use zero on Lottery pick!", BTT.UCID);
                                        }
                                        else
                                        {
                                            int RandomChance = new Random().Next(1, 10);
                                            MsgPly("^9 Winning Number is ^2" + RandomChance, BTT.UCID);

                                            #region ' Lotto '
                                            if (LottoPicked == RandomChance)
                                            {
                                                int prize = new Random().Next(2000, 4000);
                                                MsgAll("^9 " + Conn.PlayerName + " won ^2$" + prize + " ^7from winning prize in Lottery!");
                                                MsgPly("^9 Congratulations you just earned ^2$" + prize, BTT.UCID);
                                                Conn.Cash += prize;
                                            }
                                            else
                                            {
                                                int prize = new Random().Next(500, 1000);
                                                MsgAll("^9 " + Conn.PlayerName + " won a prize of ^2$" + prize + " ^7in Lottery!");
                                                MsgPly("^9 Better luck next time", BTT.UCID);
                                                Conn.Cash += prize;
                                            }
                                            #endregion

                                            Conn.LastLotto = 180;
                                            Conn.Cash -= 100;

                                            #region ' Replace Display '
                                            if (Conn.DisplaysOpen == true && Conn.InGameIntrfc == 0)
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

                                                DeleteBTN(120, BTT.UCID);
                                            }
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        if (Conn.LastLotto > 120)
                                        {
                                            MsgPly("^9 You need to wait ^1Three (3)hours^7 to rejoin the Lottery!", BTT.UCID);
                                        }
                                        else if (Conn.LastLotto > 60)
                                        {
                                            MsgPly("^9 You need to wait ^1Two (2)hours^7 to rejoin the Lottery!", BTT.UCID);
                                        }
                                        else
                                        {
                                            if (Conn.LastLotto > 1)
                                            {
                                                MsgPly("^9 You need to wait ^1" + Conn.LastLotto + "minutes^7 to rejoin the Lottery!", BTT.UCID);
                                            }
                                            else
                                            {
                                                MsgPly("^9 You need to wait ^1" + Conn.LastLotto + "minute^7 to rejoin the Lottery!", BTT.UCID);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    MsgPly("^9 Not Enough Cash to join the Lottery!", BTT.UCID);
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }
                }

                #endregion

                if (Found == false)
                {
                    MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                }
            }
            Conn.WaitCMD = 4;
        }

        [Command("sell", "sell")]
        public void sell(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (Conn.InHouse1 == false && Conn.InHouse2 == false && Conn.InHouse3 == false && Conn.InSchool == false && Conn.InShop == false && Conn.InStore == false && Conn.InBank == false)
            {
                #region ' Car Sell '
                if (StrMsg.Length == 2)
                {
                    if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains(StrMsg[1].ToUpper()))
                    {
                        if (Dealer.GetCarPrice(StrMsg[1].ToUpper()) > 0)
                        {
                            #region ' String Check '
                            string UserCars = Connections[GetConnIdx(MSO.UCID)].Cars;
                            int IdxCar = UserCars.IndexOf(StrMsg[1].ToUpper());
                            try
                            {
                                Connections[GetConnIdx(MSO.UCID)].Cars = Connections[GetConnIdx(MSO.UCID)].Cars.Remove(IdxCar, 4);
                            }
                            catch
                            {
                                Connections[GetConnIdx(MSO.UCID)].Cars = Connections[GetConnIdx(MSO.UCID)].Cars.Remove(IdxCar, 3);
                            }
                            #endregion

                            #region ' Sold Message per Car '
                            MsgAll("^9 " + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7sold ^1" + StrMsg[1].ToUpper());
                            Connections[GetConnIdx(MSO.UCID)].Cash += Dealer.GetCarValue(StrMsg[1].ToUpper());

                            if (StrMsg[1].ToUpper() == "XRG")
                            {
                                InSim.Send_MTC_MessageToConnection("^9 You have sold ^1XR GTi (XRG)", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 For: ^1$" + Dealer.GetCarValue(StrMsg[1].ToUpper()) + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash, MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "LX4")
                            {
                                InSim.Send_MTC_MessageToConnection("^9 You have sold ^1LX4 (LX4)", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 For: ^1$" + Dealer.GetCarValue(StrMsg[1].ToUpper()) + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash, MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "LX6")
                            {
                                InSim.Send_MTC_MessageToConnection("^9 You have sold ^1LX6 (LX6)", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 For: ^1$" + Dealer.GetCarValue(StrMsg[1].ToUpper()) + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash, MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "RB4")
                            {
                                InSim.Send_MTC_MessageToConnection("^9 You have sold ^1RB GT Turbo (RB4)", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 For: ^1$" + Dealer.GetCarValue(StrMsg[1].ToUpper()) + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash, MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "FXO")
                            {
                                InSim.Send_MTC_MessageToConnection("^9 You have sold ^1FX GT Turbo (FXO)", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 For: ^1$" + Dealer.GetCarValue(StrMsg[1].ToUpper()) + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash, MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "VWS")
                            {
                                InSim.Send_MTC_MessageToConnection("^9 You have sold ^1Volkswagen Scirocco (VWS)", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 For: ^1$" + Dealer.GetCarValue(StrMsg[1].ToUpper()) + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash, MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "XRT")
                            {
                                InSim.Send_MTC_MessageToConnection("^9 You have sold ^1XR GT Turbo (XRG)", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 For: ^1$" + Dealer.GetCarValue(StrMsg[1].ToUpper()) + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash, MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "RAC")
                            {
                                InSim.Send_MTC_MessageToConnection("^9 You have sold ^1Raceabout (RAC)", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 For: ^1$" + Dealer.GetCarValue(StrMsg[1].ToUpper()) + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash, MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "FZ5")
                            {
                                InSim.Send_MTC_MessageToConnection("^9 You have sold ^1FZ50 GT (FZ5)", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 For: ^1$" + Dealer.GetCarValue(StrMsg[1].ToUpper()) + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash, MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "UFR")
                            {
                                InSim.Send_MTC_MessageToConnection("^9 You have sold ^1UF GTR (UFR)", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 For: ^1$" + Dealer.GetCarValue(StrMsg[1].ToUpper()) + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash, MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "XFR")
                            {
                                InSim.Send_MTC_MessageToConnection("^9 You have sold ^1XF GTR (XFR)", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 For: ^1$" + Dealer.GetCarValue(StrMsg[1].ToUpper()) + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash, MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "FXR")
                            {
                                InSim.Send_MTC_MessageToConnection("^9 You have sold ^1FX GTR (FXR)", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 For: ^1$" + Dealer.GetCarValue(StrMsg[1].ToUpper()) + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash, MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "XRR")
                            {
                                InSim.Send_MTC_MessageToConnection("^9 You have sold ^1XR GTR (XRR)", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 For: ^1$" + Dealer.GetCarValue(StrMsg[1].ToUpper()) + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash, MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "FZR")
                            {
                                InSim.Send_MTC_MessageToConnection("^9 You have sold ^1FZ GTR (FZR)", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 For: ^1$" + Dealer.GetCarValue(StrMsg[1].ToUpper()) + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash, MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "MRT")
                            {
                                InSim.Send_MTC_MessageToConnection("^9 You have sold ^1McGill Racing Kart 5 (MRT)", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 For: ^1$" + Dealer.GetCarValue(StrMsg[1].ToUpper()) + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash, MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "FBM")
                            {
                                InSim.Send_MTC_MessageToConnection("^9 You have sold ^1Formula BMW FBO2 (FBM)", MSO.UCID, 0);
                                InSim.Send_MTC_MessageToConnection("^9 For: ^1$" + Dealer.GetCarValue(StrMsg[1].ToUpper()) + " ^7You have: ^2$" + Connections[GetConnIdx(MSO.UCID)].Cash, MSO.UCID, 0);
                            }
                            #endregion
                        }

                        #region ' Check if Soldable '
                        else
                        {
                            if (StrMsg[1].ToUpper() == "UF1")
                            {
                                InSim.Send_MTC_MessageToConnection("^4| UF1000 (UF1) ^7cannot be sold", MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "XFG")
                            {
                                InSim.Send_MTC_MessageToConnection("^4| XF GTi (XFG) ^7cannot be sold", MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "FO8")
                            {
                                InSim.Send_MTC_MessageToConnection("^4| Formula V8 (FO8) ^7cannot be sold", MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "FOX")
                            {
                                InSim.Send_MTC_MessageToConnection("^4| Formula XR (FOX) ^7cannot be sold", MSO.UCID, 0);
                            }
                            else if (StrMsg[1].ToUpper() == "BF1")
                            {
                                InSim.Send_MTC_MessageToConnection("^4| BMW Sauber F1.06 (BF1) ^7cannot be sold", MSO.UCID, 0);
                            }
                            else
                            {
                                InSim.Send_MTC_MessageToConnection("^4| " + StrMsg[1].ToUpper() + " ^7Invalid Car Garage List", MSO.UCID, 0);
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                }
                #endregion
            }
            else
            {
                #region ' Trading '
                bool Found = false;
                if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true)
                {
                    if (StrMsg.Length == 3)
                    {
                        Found = true;
                        if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                        {
                            if (Conn.IsOfficer == false && Conn.IsCadet == false && Conn.IsTowTruck == false)
                            {
                                byte Amount = byte.Parse(StrMsg[2]);

                                #region ' Electronic '
                                if (StrMsg[1] == "electronic")
                                {
                                    if (StrMsg[2].Contains("-"))
                                    {
                                        MsgPly("^9 Trade Incorrect. Don't put minus on the values!", MSO.UCID);
                                    }
                                    else
                                    {
                                        if (Amount < 6)
                                        {
                                            if (Conn.Electronics > Amount)
                                            {
                                                Conn.Electronics -= Amount;
                                                Conn.Cash += Amount * Conn.SellElectronics;

                                                MsgAll("^9 " + Conn.PlayerName + " traded their Electronic for ^3" + Amount + "^7!");
                                                MsgAll("^9 " + Conn.PlayerName + " Got paid for ^2$" + Amount * Conn.SellElectronics);

                                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                                {
                                                    if (Conn.Electronics > 0)
                                                    {
                                                        InSim.Send_BTN_CreateButton("^2Trade ^7your Electronic Items for ^1$" + Conn.SellElectronics + " ^7each.", Flags.ButtonStyles.ISB_LEFT, 4, 100, 65, 54, 114, Conn.UniqueID, 2, false);
                                                        InSim.Send_BTN_CreateButton("^2Trade", "Maximum Trading Items 5 each.", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 65, 100, 1, 118, Conn.UniqueID, 2, false);
                                                    }
                                                    else
                                                    {
                                                        InSim.Send_BTN_CreateButton("^2You don't ^7have enough items to Trade any Electronic!", Flags.ButtonStyles.ISB_LEFT, 4, 100, 65, 54, 114, Conn.UniqueID, 2, false);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                MsgPly("^9 Not Enough Electronics to Trade!", MSO.UCID);
                                            }
                                        }
                                        else
                                        {
                                            MsgPly("^9 Trading Electronics More than 5 is not Allowed.", MSO.UCID);
                                        }
                                    }
                                }
                                #endregion

                                #region ' Furniture '
                                if (StrMsg[1] == "furniture")
                                {
                                    if (StrMsg[1].Contains("-"))
                                    {
                                        MsgPly("^9 Trade Incorrect. Don't put minus on the values!", MSO.UCID);
                                    }
                                    else
                                    {
                                        if (Amount < 6)
                                        {
                                            if (Conn.Furniture > Amount)
                                            {
                                                Conn.Furniture -= Amount;
                                                Conn.Cash += Amount * Conn.Furniture;

                                                MsgAll("^9 " + Conn.PlayerName + " traded their Furniture for ^3" + Amount + "^7!");
                                                MsgAll("^9 " + Conn.PlayerName + " Got paid for ^2$" + Amount * Conn.SellFurniture);


                                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                                {
                                                    if (Conn.Furniture > 0)
                                                    {
                                                        InSim.Send_BTN_CreateButton("^2Trade ^7your Furniture Items for ^1$" + Conn.SellElectronics + " ^7each.", Flags.ButtonStyles.ISB_LEFT, 4, 100, 69, 54, 115, Conn.UniqueID, 2, false);
                                                        InSim.Send_BTN_CreateButton("^2Trade", "Maximum Trading Items 5 each.", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 69, 100, 1, 119, Conn.UniqueID, 2, false);
                                                    }
                                                    else
                                                    {
                                                        InSim.Send_BTN_CreateButton("^2You don't ^7have enough items to trade any Furniture!", Flags.ButtonStyles.ISB_LEFT, 4, 100, 69, 54, 115, Conn.UniqueID, 2, false);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                MsgPly("^9 Not Enough Furniture to Trade!", MSO.UCID);
                                            }
                                        }
                                        else
                                        {
                                            MsgPly("^9 Trading Furniture More than 5 is not Allowed.", MSO.UCID);
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                MsgPly("^9 Can't sell/trade whilst being duty!", MSO.UCID);
                            }
                        }
                        else
                        {
                            MsgPly("^9 Can't sell/trade whilst being chased!", MSO.UCID);
                        }
                    }
                }
                if (Found == false)
                {
                    MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                    MsgPly("^7If you want to sell your cars move away in house!", MSO.UCID);
                }
                #endregion
            }
        }

        #endregion

        #region ' Cop and TowTruck Commands '

        [Command("duty", "duty")]
        public void duty(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length == 1)
            {
                if (Conn.InGame == 0)
                {
                    MsgPly("^9 You must be in vehicle before you access this command!", MSO.UCID);
                }
                else if (Conn.JobToHouse1 == false && Conn.JobToHouse2 == false && Conn.JobToHouse3 == false && Conn.JobToSchool == false)
                {
                    #region ' Officer Duty '
                    if (Conn.PlayerName.Contains(OfficerTag))
                    {
                        if (Conn.CanBeOfficer == 1)
                        {
                            if (Conn.Plate == "iC PD")
                            {
                                if (Conn.IsOfficer == false)
                                {
                                    MsgAll("^9 " + Conn.PlayerName + " is now ^2DUTY ^7as a Cop!");
                                    Conn.LastName = Conn.PlayerName;

                                    if (Conn.CopPanel == 0)
                                    {
                                        MsgPly("^9 Your Panel Click is disabled", MSO.UCID);
                                        MsgPly("  ^7To Enable them by typing ^2!coppanel", MSO.UCID);
                                    }
                                    else if (Conn.CopPanel == 1)
                                    {
                                        MsgPly("^9 Your Panel Click is enabled", MSO.UCID);
                                        MsgPly("  ^7To Disable them by typing ^2!coppanel", MSO.UCID);
                                    }

                                    #region ' Close BTN '
                                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                    {
                                        if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true)
                                        {
                                            DeleteBTN(110, Conn.UniqueID);
                                            DeleteBTN(111, Conn.UniqueID);
                                            DeleteBTN(112, Conn.UniqueID);
                                            DeleteBTN(113, Conn.UniqueID);
                                            DeleteBTN(114, Conn.UniqueID);
                                            DeleteBTN(115, Conn.UniqueID);
                                            DeleteBTN(116, Conn.UniqueID);
                                            DeleteBTN(117, Conn.UniqueID);
                                            DeleteBTN(118, Conn.UniqueID);
                                            DeleteBTN(119, Conn.UniqueID);
                                            DeleteBTN(120, Conn.UniqueID);
                                            DeleteBTN(121, Conn.UniqueID);
                                            Conn.DisplaysOpen = false;
                                        }
                                        if (Conn.InShop == true || Conn.InStore == true)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Can't take a job whilst duty!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                            DeleteBTN(121, Conn.UniqueID);
                                        }
                                    }
                                    #endregion

                                    TotalOfficers += 1;
                                    Conn.IsOfficer = true;
                                    Conn.LastName = Conn.PlayerName;
                                }
                                else
                                {
                                    MsgPly("^9 You are now duty as a Cop.", MSO.UCID);
                                    MsgPly("  ^7If you want to get ^1OFF-DUTY ^7please remove the Tag!", MSO.UCID);
                                }
                            }
                            else
                            {
                                MsgPly("^9 Your Platenumber must be iC PD!", MSO.UCID);
                            }
                        }
                        else
                        {
                            MsgPly("^9 Not Authorized Cop!", MSO.UCID);
                        }
                    }
                    #endregion

                    #region ' Cadet Duty '
                    if (Conn.PlayerName.Contains(CadetTag))
                    {
                        if (Conn.CanBeCadet == 1)
                        {
                            if (Conn.Plate == "iC PD")
                            {
                                if (Conn.IsCadet == false)
                                {
                                    MsgAll("^9 " + Conn.PlayerName + " is now ^2DUTY ^7as a Cop!");
                                    Conn.LastName = Conn.PlayerName;

                                    if (Conn.CopPanel == 0)
                                    {
                                        MsgPly("^9 Your Panel Click is disabled", MSO.UCID);
                                        MsgPly("  ^7To Enable them by typing ^2!coppanel", MSO.UCID);
                                    }
                                    else if (Conn.CopPanel == 1)
                                    {
                                        MsgPly("^9 Your Panel Click is enabled", MSO.UCID);
                                        MsgPly("  ^7To Disable them by typing ^2!coppanel", MSO.UCID);
                                    }

                                    #region ' Close BTN '
                                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                    {
                                        if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true)
                                        {
                                            DeleteBTN(110, Conn.UniqueID);
                                            DeleteBTN(111, Conn.UniqueID);
                                            DeleteBTN(112, Conn.UniqueID);
                                            DeleteBTN(113, Conn.UniqueID);
                                            DeleteBTN(114, Conn.UniqueID);
                                            DeleteBTN(115, Conn.UniqueID);
                                            DeleteBTN(116, Conn.UniqueID);
                                            DeleteBTN(117, Conn.UniqueID);
                                            DeleteBTN(118, Conn.UniqueID);
                                            DeleteBTN(119, Conn.UniqueID);
                                            DeleteBTN(120, Conn.UniqueID);
                                            DeleteBTN(121, Conn.UniqueID);
                                            Conn.DisplaysOpen = false;
                                        }
                                        if (Conn.InShop == true || Conn.InStore == true)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Can't take a job whilst duty!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                            DeleteBTN(121, Conn.UniqueID);
                                        }
                                    }
                                    #endregion

                                    Conn.IsCadet = true;
                                    Conn.LastName = Conn.PlayerName;
                                }
                                else
                                {
                                    MsgPly("^9 You are now duty as a Cop.", MSO.UCID);
                                    MsgPly("  ^7If you want to get ^1OFF-DUTY ^7please remove the Tag!", MSO.UCID);
                                }
                            }
                            else
                            {
                                MsgPly("^9 Your Platenumber must be iC PD!", MSO.UCID);
                            }
                        }
                        else
                        {
                            MsgPly("^9 Not Authorized Cop!", MSO.UCID);
                        }
                    }
                    #endregion

                    #region ' Tow Truck Duty '
                    if (Conn.PlayerName.Contains(TowTruckTag))
                    {
                        if (Conn.CanBeTowTruck == 1)
                        {
                            if (Conn.Plate == "iC Tow")
                            {
                                if (Conn.CurrentCar == "FBM")
                                {
                                    MsgPly("^9 You cannot get duty whilst using FBM!", MSO.UCID);
                                }

                                else if (Conn.IsTowTruck == false)
                                {
                                    MsgAll("^9 " + Conn.PlayerName + " is now ^2ON-DUTY ^7as Tow Truck!");

                                    #region ' Close BTN '
                                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                    {
                                        if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true)
                                        {
                                            DeleteBTN(110, Conn.UniqueID);
                                            DeleteBTN(111, Conn.UniqueID);
                                            DeleteBTN(112, Conn.UniqueID);
                                            DeleteBTN(113, Conn.UniqueID);
                                            DeleteBTN(114, Conn.UniqueID);
                                            DeleteBTN(115, Conn.UniqueID);
                                            DeleteBTN(116, Conn.UniqueID);
                                            DeleteBTN(117, Conn.UniqueID);
                                            DeleteBTN(118, Conn.UniqueID);
                                            DeleteBTN(119, Conn.UniqueID);
                                            DeleteBTN(120, Conn.UniqueID);
                                            DeleteBTN(121, Conn.UniqueID);
                                            Conn.DisplaysOpen = false;
                                        }
                                        if (Conn.InShop == true || Conn.InStore == true)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Can't take a job whilst duty!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                            DeleteBTN(121, Conn.UniqueID);
                                        }
                                    }
                                    #endregion

                                    if (Conn.CalledRequest == true)
                                    {
                                        Conn.CalledRequest = false;
                                    }

                                    Conn.LastName = Conn.PlayerName;
                                    Conn.IsTowTruck = true;
                                }
                                else
                                {
                                    MsgPly("^9 You are now duty as a TowTruck.", MSO.UCID);
                                    MsgPly("  ^7If you want to get ^1OFF-DUTY ^7please remove the Tag!", MSO.UCID);
                                }
                            }
                            else
                            {
                                MsgPly("^9 Your Platenumber must be in ' iC PD '!", MSO.UCID);
                            }
                        }
                        else
                        {
                            MsgPly("^9 Not Authorized Tow Truck!", MSO.UCID);
                        }
                    }
                    #endregion
                }
                else
                {
                    MsgPly("^9 Finish or Cancel your current Job!", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        #region ' Cops Command '

        [Command("settrap", "settrap <kmh>")]
        public void settrap(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (Conn.IsOfficer == true && Conn.CanBeOfficer == 1)
            {
                if (StrMsg.Length == 2)
                {
                    if (Conn.InGame == 0)
                    {
                        MsgPly("^9 You must be in vehicle before you access this command!", MSO.UCID);
                    }
                    else if (Conn.InChaseProgress == false)
                    {
                        try
                        {
                            int TrapSpeed = Convert.ToInt32(StrMsg[1]);

                            if (TrapSpeed.ToString().Contains("-"))
                            {
                                MsgPly("^9 Invalid Input. Don't put minus values!", MSO.UCID);
                            }
                            else
                            {
                                if (Conn.TrapSetted == false)
                                {
                                    if (Conn.CompCar.Speed / 91 < 3)
                                    {
                                        var BTT = MSO;
                                        if (TrapSpeed > 50 && TrapSpeed < 230)
                                        {
                                            Conn.TrapX = Conn.CompCar.X / 196608;
                                            Conn.TrapY = Conn.CompCar.Y / 196608;
                                            Conn.TrapSpeed = TrapSpeed;
                                            foreach (clsConnection i in Connections)
                                            {
                                                if (i.IsOfficer == true && i.CanBeOfficer == 1 || i.IsCadet == true && i.CanBeCadet == 1)
                                                {
                                                    MsgPly("^9 " + Conn.PlayerName + " set a Trap", i.UniqueID);
                                                    MsgPly("^9 Located: " + Conn.Location, i.UniqueID);
                                                    MsgPly("^9 Trap Setted at X: ^3" + Conn.TrapX + " ^7Y: ^3" + Conn.TrapY, i.UniqueID);
                                                    MsgPly("^9 Speed: ^3" + Conn.TrapSpeed + " kmh", i.UniqueID);
                                                }
                                            }
                                            Conn.TrapSetted = true;
                                        }
                                        else
                                        {
                                            if (TrapSpeed < 50)
                                            {
                                                MsgPly("^9 Speed Traps can't be setted lower 50kmh!", BTT.UCID);
                                            }
                                            else if (TrapSpeed > 230)
                                            {
                                                MsgPly("^9 Speed Traps can't be setted more than 230kmh!", BTT.UCID);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^9 Can't Set a Trap whilst being driving!", MSO.UCID);
                                    }
                                }
                                else
                                {
                                    MsgPly("^9 The Trap has been setted in this Area!", MSO.UCID);
                                }
                            }
                        }
                        catch
                        {
                            MsgPly("^9 Trap Error. Please check your values!", MSO.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^9 Can't set a Trap whilst in chase progress!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^9 Not Authorized Officer!", MSO.UCID);
            }

        }

        [Command("remtrap", "remtrap")]
        public void remtrap(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (Conn.IsOfficer == true && Conn.CanBeOfficer == 1)
            {
                if (StrMsg.Length == 1)
                {
                    if (Conn.TrapSetted == true)
                    {
                        MsgPly("^9 Speed Trap Removed", Conn.UniqueID);
                        Conn.TrapY = 0;
                        Conn.TrapX = 0;
                        Conn.TrapSpeed = 0;
                        Conn.TrapSetted = false;
                    }
                    else
                    {
                        MsgPly("^9 No Trap has been Setted!", Conn.UniqueID);
                    }
                }
                else
                {
                    MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^9 Not Authorized Officer!", MSO.UCID);
            }
        }

        [Command("chase", "chase")]
        public void chase(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(MSO.UCID)].Chasee)];
            if (StrMsg.Length == 1)
            {
                #region ' Engage! '
                if (Conn.CanBeOfficer == 1 || Conn.CanBeCadet == 1)
                {
                    if (Conn.IsOfficer == true || Conn.IsCadet == true)
                    {
                        if (Conn.InGame == 1)
                        {
                            #region ' Engage '
                            if (Conn.InChaseProgress == false)
                            {
                                try
                                {
                                    if (Conn.UniqueID == MSO.UCID)
                                    {
                                        #region ' Object Variables '
                                        int LowestDistance = 250;
                                        byte ChaseeIndex = 0;
                                        int Distance = 0;
                                        int ChaseeUCID = -1;
                                        #endregion

                                        #region ' Chase Setup '
                                        for (int i = 0; i < Connections.Count; i++)
                                        {
                                            if (Connections[i].PlayerID != 0)
                                            {
                                                Distance = ((int)Math.Sqrt(Math.Pow(Connections[i].CompCar.X - Conn.CompCar.X, 2) + Math.Pow(Connections[i].CompCar.Y - Conn.CompCar.Y, 2)) / 65536);
                                                Connections[i].DistanceFromCop = Distance;
                                            }
                                        }
                                        for (int i = 0; i < Connections.Count; i++)
                                        {
                                            if (Connections[i].PlayerID != 0)
                                            {
                                                if ((Connections[i].DistanceFromCop < LowestDistance) && (Connections[i].DistanceFromCop > 0) && (Connections[i].PlayerName != Conn.PlayerName) && (Connections[i].IsOfficer == false) && (Connections[i].IsCadet == false))
                                                {
                                                    LowestDistance = Connections[i].DistanceFromCop;

                                                    ChaseeUCID = Connections[i].UniqueID;
                                                    ChaseeIndex = (byte)i;
                                                }
                                            }
                                        }
                                        #endregion

                                        #region ' Detect '

                                        if (Conn.PlayerName == HostName == false)
                                        {
                                            if ((LowestDistance < 150) && (Connections[GetConnIdx(ChaseeUCID)].DistanceFromCop > 0))
                                            {
                                                #region ' New Engage '
                                                if (Connections[ChaseeIndex].IsSuspect == false)
                                                {
                                                    if (ChaseLimit == AddChaseLimit && Conn.InChaseProgress == false)
                                                    {
                                                        MsgPly("^9 Maximum Pursuit Limit: ^7" + AddChaseLimit, MSO.UCID);
                                                    }
                                                    else
                                                    {
                                                        #region ' Start Chase '
                                                        if (Connections[ChaseeIndex].IsBeingBusted == false)
                                                        {
                                                            Conn.Chasee = ChaseeUCID;
                                                            Conn.InChaseProgress = true;
                                                            Conn.ChaseCondition = 1;
                                                            Conn.AutoBumpTimer = 50;
                                                            Connections[ChaseeIndex].CopInChase = 1;
                                                            Connections[ChaseeIndex].ChaseCondition = 1;
                                                            Connections[ChaseeIndex].PullOvrMsg = 30;
                                                            AddChaseLimit += 1;

                                                            MsgAll("^9 " + Conn.PlayerName + " ^3started a chase!");
                                                            MsgAll("^9 Suspect Name : ^7" + Connections[ChaseeIndex].PlayerName + " (" + Connections[ChaseeIndex].Username + ")");
                                                            MsgAll("^9 Chase Condition : ^7" + Connections[ChaseeIndex].ChaseCondition);

                                                            Connections[ChaseeIndex].IsSuspect = true;
                                                        }
                                                        else
                                                        {
                                                            MsgPly("^9 " + Connections[ChaseeIndex].PlayerName + " is being busted a cop.", MSO.UCID);
                                                        }
                                                        #endregion
                                                    }
                                                }
                                                #endregion

                                                #region ' Join Chase '
                                                else if (Connections[ChaseeIndex].IsSuspect == true && Connections[ChaseeIndex].ChaseCondition >= 2)
                                                {
                                                    if (Conn.InChaseProgress == false)
                                                    {
                                                        Connections[ChaseeIndex].CopInChase += 1;
                                                        Conn.ChaseCondition = Connections[ChaseeIndex].ChaseCondition;
                                                        Conn.Chasee = ChaseeUCID;
                                                        Conn.JoinedChase = true;
                                                        Conn.InChaseProgress = true;

                                                        #region ' Connection List '
                                                        foreach (clsConnection Con in Connections)
                                                        {
                                                            if (Con.Chasee == Connections[ChaseeIndex].UniqueID)
                                                            {
                                                                Conn.BumpButton = Con.BumpButton;
                                                            }
                                                        }
                                                        #endregion

                                                        MsgAll("^9 " + Conn.PlayerName + " ^3joins in backup chase!");
                                                        MsgAll("^9 Suspect Name: " + Connections[ChaseeIndex].PlayerName + " (" + Connections[ChaseeIndex].Username + ")");
                                                        MsgAll("^9 Cops In Chase: " + Connections[ChaseeIndex].CopInChase);
                                                        MsgAll("^9 Chase Condition: ^7" + Connections[ChaseeIndex].ChaseCondition);
                                                        Connections[ChaseeIndex].IsSuspect = true;
                                                    }
                                                }
                                                else
                                                {
                                                    MsgPly("^9 Cannot join in the Police Pursuit.", MSO.UCID);
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                MsgPly("^9 No Civilian found in 150 meters", MSO.UCID);
                                            }
                                        }

                                        #endregion
                                    }
                                }
                                catch
                                {
                                    MsgPly("^9 Engage Error.", MSO.UCID);
                                }
                            }
                            #endregion

                            #region ' Bump '
                            else if (Conn.InChaseProgress == true)
                            {
                                if (Conn.Busted == false)
                                {
                                    if (Conn.AutoBumpTimer == 0)
                                    {
                                        if (Conn.JoinedChase == true)
                                        {
                                            MsgPly("^9 You can't extend the Condition without the Leader increase!", Conn.UniqueID);
                                        }
                                        else if (Conn.ChaseCondition > 0 && Conn.ChaseCondition < 5 && Conn.JoinedChase == false)
                                        {

                                            #region ' Cops In chase! '
                                            if (ChaseCon.CopInChase > 1)
                                            {
                                                foreach (clsConnection Con in Connections)
                                                {
                                                    if (Con.Chasee == ChaseCon.UniqueID)
                                                    {
                                                        MsgAll("^9 " + Con.PlayerName + " ^3still chasing ^7" + ChaseCon.PlayerName + "!");
                                                        Con.BumpButton += 1;
                                                    }
                                                }
                                                MsgAll("^9 Cops In Chase: ^7" + ChaseCon.CopInChase);
                                            }
                                            else if (ChaseCon.CopInChase == 1)
                                            {
                                                MsgAll("^9 " + Conn.PlayerName + " ^3still chasing ^7" + ChaseCon.PlayerName + "!");
                                                Conn.BumpButton += 1;
                                            }
                                            #endregion

                                            #region ' Chase Condition '
                                            switch (Conn.BumpButton)
                                            {
                                                case 1:

                                                    MsgAll("^9 Suspect Name : ^7" + ChaseCon.PlayerName + " (" + ChaseCon.Username + ")");
                                                    MsgAll("^9 Chase Condition : ^72");
                                                    InSim.Send_MTC_MessageToConnection("^9 YOU HAVE REACHED LEVEL 2 OF CHASING!", ChaseCon.UniqueID, 0);


                                                    Conn.ChaseCondition = 2;
                                                    ChaseCon.ChaseCondition = 2;
                                                    Conn.AutoBumpTimer = 70;

                                                    #region ' Connection List '
                                                    foreach (clsConnection C in Connections)
                                                    {
                                                        if (C.Chasee == ChaseCon.UniqueID)
                                                        {
                                                            if (C.JoinedChase == true)
                                                            {
                                                                C.ChaseCondition = 2;
                                                            }
                                                        }
                                                    }
                                                    #endregion


                                                    break;

                                                case 2:

                                                    MsgAll("^9 Suspect Name : ^7" + ChaseCon.PlayerName + " (" + ChaseCon.Username + ")");
                                                    MsgAll("^9 Chase Condition : ^73");
                                                    InSim.Send_MTC_MessageToConnection("^9 YOU HAVE REACHED LEVEL 3 OF CHASING!", ChaseCon.UniqueID, 0);

                                                    Conn.ChaseCondition = 3;
                                                    ChaseCon.ChaseCondition = 3;

                                                    #region ' Connection List '
                                                    foreach (clsConnection C in Connections)
                                                    {
                                                        if (C.Chasee == ChaseCon.UniqueID)
                                                        {
                                                            if (C.JoinedChase == true)
                                                            {
                                                                C.ChaseCondition = 3;
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    Conn.AutoBumpTimer = 80;
                                                    break;

                                                case 3:

                                                    MsgAll("^9 Suspect Name : ^7" + ChaseCon.PlayerName + " (" + ChaseCon.Username + ")");
                                                    MsgAll("^9 Chase Condition : ^74");

                                                    InSim.Send_MTC_MessageToConnection("^9 YOU HAVE REACHED LEVEL 4 OF CHASING!", ChaseCon.UniqueID, 0);

                                                    Conn.ChaseCondition = 4;
                                                    ChaseCon.ChaseCondition = 4;

                                                    #region ' Connection List '
                                                    foreach (clsConnection C in Connections)
                                                    {
                                                        if (C.Chasee == ChaseCon.UniqueID)
                                                        {
                                                            if (C.JoinedChase == true)
                                                            {
                                                                C.ChaseCondition = 4;
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    Conn.AutoBumpTimer = 90;
                                                    break;


                                                case 4:

                                                    MsgAll("^9 Suspect Name : ^7" + ChaseCon.PlayerName + " (" + ChaseCon.Username + ")");
                                                    MsgAll("^9 Chase Condition : ^75");


                                                    InSim.Send_MTC_MessageToConnection("^9 YOU HAVE REACHED THE FINAL LEVEL OF CHASING!", ChaseCon.UniqueID, 0);
                                                    Conn.ChaseCondition = 5;
                                                    ChaseCon.ChaseCondition = 5;

                                                    #region ' Connection List '
                                                    foreach (clsConnection C in Connections)
                                                    {
                                                        if (C.Chasee == ChaseCon.UniqueID)
                                                        {
                                                            if (C.JoinedChase == true)
                                                            {
                                                                C.ChaseCondition = 5;
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    break;
                                            }
                                            #endregion
                                        }
                                        else if (Conn.ChaseCondition == 5)
                                        {
                                            MsgPly("^9 Chase Condition is already reached the Final", Conn.UniqueID);
                                        }
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

                                        MsgPly("^9 Wait for the Bump timer for ^2" + Minutes + ":" + Seconds, Conn.UniqueID);
                                        MsgPly("^7  To Increase the Condition!", Conn.UniqueID);
                                    }
                                }
                                else if (Conn.Busted == true)
                                {
                                    MsgPly("^9 type ^2!busted ^7to busted the suspect", MSO.UCID);
                                    MsgPly("^7  or ^2!disengage ^7to stop the chase!", MSO.UCID);
                                }
                            }
                            #endregion
                        }
                        else if (Conn.InGame == 0)
                        {
                            MsgPly("^9 You must be in vehicle before you access this command!", MSO.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^9 You must be duty before you can start a Chase!", MSO.UCID);
                    }
                }
                #endregion

                #region ' Not Authorized or Failed '
                else
                {
                    MsgPly("^9 Not Authorized.", MSO.UCID);
                }
                #endregion
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("disengage", "disengage")]
        public void disengage(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(MSO.UCID)].Chasee)];
            if (StrMsg.Length == 1)
            {
                if (Conn.CanBeOfficer == 1 || Conn.CanBeCadet == 1)
                {
                    if (Conn.IsOfficer == true || Conn.IsCadet == true)
                    {
                        if (Conn.InFineMenu == false)
                        {
                            if (Conn.InGame == 1)
                            {
                                if (Conn.ChaseCondition != 0)
                                {
                                    MsgAll("^9 " + Conn.PlayerName + " ^3ends chase on ^7" + ChaseCon.PlayerName + "!");

                                    #region ' Disengage Joined in chase '
                                    if (ChaseCon.CopInChase > 1)
                                    {
                                        if (Conn.JoinedChase == true)
                                        {
                                            Conn.JoinedChase = false;
                                        }
                                        Conn.ChaseCondition = 0;
                                        Conn.Busted = false;
                                        Conn.InChaseProgress = false;
                                        Conn.BustedTimer = 0;
                                        Conn.BumpButton = 0;
                                        Conn.Chasee = -1;
                                        ChaseCon.CopInChase -= 1;

                                        #region ' Connection List '
                                        if (ChaseCon.CopInChase == 1)
                                        {
                                            foreach (clsConnection Con in Connections)
                                            {
                                                if (Con.Chasee == ChaseCon.UniqueID)
                                                {
                                                    if (Con.JoinedChase == true)
                                                    {
                                                        Con.JoinedChase = false;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region ' Disengage '
                                    else if (ChaseCon.CopInChase == 1)
                                    {
                                        AddChaseLimit -= 1;
                                        Conn.AutoBumpTimer = 0;
                                        Conn.BumpButton = 0;
                                        Conn.BustedTimer = 0;
                                        Conn.Chasee = -1;
                                        Conn.Busted = false;
                                        Conn.InChaseProgress = false;
                                        ChaseCon.PullOvrMsg = 0;
                                        ChaseCon.ChaseCondition = 0;
                                        ChaseCon.CopInChase = 0;
                                        ChaseCon.IsSuspect = false;
                                        Conn.ChaseCondition = 0;
                                        CopSirenShutOff();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    MsgPly("^9 You aren't in chase!", MSO.UCID);
                                }
                            }
                            else if (Conn.InGame == 0)
                            {
                                MsgPly("^9 You must be in vehicle before you access this command!", MSO.UCID);
                            }
                        }
                        else if (Conn.InFineMenu == true)
                        {
                            MsgPly("^9 Set a Tickets to " + ChaseCon.PlayerName + "!", MSO.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^9 You must be duty before you can access this command!", MSO.UCID);
                    }
                }
                #region ' Not Authorized or Failed '
                else
                {
                    MsgPly("^9 Not Authorized.", MSO.UCID);
                }
                #endregion
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("busted", "busted")]
        public void busted(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(MSO.UCID)].Chasee)];

            if (StrMsg.Length == 1)
            {
                if (Conn.CanBeOfficer == 1 || Conn.CanBeCadet == 1)
                {
                    if (Conn.IsOfficer == true || Conn.IsCadet == true)
                    {
                        if (Conn.InGame == 1)
                        {
                            if (Conn.InChaseProgress == true)
                            {
                                #region ' Enabled Busted '
                                if (Conn.Busted == true)
                                {
                                    if (Conn.BustedTimer == 5)
                                    {
                                        MsgAll("^9 " + Conn.PlayerName + " busts the suspect!");
                                        MsgAll("^9 Suspect Name: " + ChaseCon.PlayerName + " (" + ChaseCon.Username + ")");
                                        if (ChaseCon.CopInChase > 1)
                                        {
                                            MsgAll("^9 Cops In Chase: " + ChaseCon.CopInChase);
                                        }
                                        MsgAll("^9 Suspect Chase Condition: " + Conn.ChaseCondition);
                                        MsgPly("^9 Don't move away whilst being busted!", ChaseCon.UniqueID);
                                        MsgPly("^9 Please wait to receive your Ticket!", ChaseCon.UniqueID);

                                        #region ' List of Connection Joined Chase '
                                        foreach (clsConnection Con in Connections)
                                        {
                                            if (Con.Chasee == ChaseCon.UniqueID)
                                            {
                                                if (ChaseCon.CopInChase > 1)
                                                {
                                                    if (Con.JoinedChase == true && Con.Busted == false)
                                                    {
                                                        MsgPly("^9 " + Conn.PlayerName + " busts the suspect.", Con.UniqueID);
                                                        MsgPly("^9 Please wait to get your rewards!", Con.UniqueID);

                                                    }
                                                    else if (Con.JoinedChase == false && Con.Busted == false)
                                                    {
                                                        MsgPly("^9 " + Conn.PlayerName + " busts the suspect.", Con.UniqueID);
                                                        MsgPly("^9 Please wait to get your rewards!", Con.UniqueID);

                                                    }
                                                    Con.Busted = true;
                                                    Con.BustedTimer = 0;
                                                    Con.AutoBumpTimer = 0;
                                                    Con.BumpButton = 0;
                                                    Con.InChaseProgress = false;
                                                }
                                                else if (ChaseCon.CopInChase == 1)
                                                {
                                                    Con.Busted = true;
                                                    Con.InFineMenu = true;
                                                    Con.AutoBumpTimer = 0;
                                                    Con.BumpButton = 0;
                                                    Con.BustedTimer = 0;
                                                    Con.InChaseProgress = false;
                                                }
                                            }
                                        }
                                        #endregion

                                        InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 30, (Conn.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 31, (Conn.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Ticket Window", Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 32, (Conn.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Suspect Name: " + ChaseCon.PlayerName, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 60, 54, 33, (Conn.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Chase Condition: " + Conn.ChaseCondition, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 65, 54, 34, (Conn.UniqueID), 2, false);

                                        #region ' Condition '
                                        if (Conn.ChaseCondition == 1)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$500", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 54, 35, (Conn.UniqueID), 2, false);
                                        }
                                        if (Conn.ChaseCondition == 2)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$1,300", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 58, 35, (Conn.UniqueID), 2, false);
                                        }
                                        if (Conn.ChaseCondition == 3)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$2,500", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 58, 35, (Conn.UniqueID), 2, false);
                                        }
                                        if (Conn.ChaseCondition == 4)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$3,500", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 58, 35, (Conn.UniqueID), 2, false);
                                        }
                                        if (Conn.ChaseCondition == 5)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$5,000", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 58, 35, (Conn.UniqueID), 2, false);
                                        }
                                        #endregion

                                        // Click Buttons
                                        InSim.Send_BTN_CreateButton("^7Reason", "Enter the chase reason", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 78, 77, 64, 36, (Conn.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Enter a Fine Amount And Reason For The Chase", Flags.ButtonStyles.ISB_C1, 4, 70, 95, 65, 38, (Conn.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Warn", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 4, 10, 103, 107, 39, (Conn.UniqueID), 40, false);
                                        InSim.Send_BTN_CreateButton("^7Issue", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 4, 10, 103, 82, 40, (Conn.UniqueID), 40, false);

                                        Conn.InFineMenu = true;
                                        AddChaseLimit -= 1;
                                        ChaseCon.IsSuspect = false;
                                        ChaseCon.IsBeingBusted = true;
                                        ChaseCon.ChaseCondition = 0;
                                        CopSirenShutOff();
                                    }
                                }
                                #endregion

                                else if (Conn.Busted == false)
                                {
                                    if (ChaseCon.IsBeingBusted == true)
                                    {
                                        MsgPly("^9 " + ChaseCon.PlayerName + " being fined at the momment!", MSO.UCID);
                                        MsgPly("  ^7Please wait for awhile when the fine is accepted or refused", MSO.UCID);
                                    }
                                    else if (ChaseCon.IsBeingBusted == false)
                                    {
                                        MsgPly("^9 You must pull over " + ChaseCon.PlayerName + " to busted!", MSO.UCID);
                                    }
                                }
                            }
                            else if (Conn.InChaseProgress == false)
                            {
                                MsgPly("^9 You aren't in chase!", MSO.UCID);
                            }
                        }

                        else if (Conn.InGame == 0)
                        {
                            MsgPly("^9 You must be in vehicle before you access this command!", MSO.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^9 You must be duty before you can access this command!", MSO.UCID);
                    }
                }
                #region ' Not Authorized or Failed '
                else
                {
                    MsgPly("^9 Not Authorized.", MSO.UCID);
                }
                #endregion
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("backup", "backup")]
        public void backup(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(MSO.UCID)].Chasee)];
            if (StrMsg.Length == 1)
            {
                if (Conn.CanBeOfficer == 1 || Conn.CanBeCadet == 1)
                {
                    if (Conn.IsOfficer == true || Conn.IsCadet == true)
                    {
                        if (Conn.InGame == 1)
                        {
                            if (Conn.ChaseCondition == 1)
                            {
                                MsgPly("^9 You cannot call a backup whilst in Conditon 1!", MSO.UCID);
                            }
                            else if (Conn.ChaseCondition > 1)
                            {
                                bool Found = false;

                                foreach (clsConnection i in Connections)
                                {
                                    if (i.IsCadet == true && i.InChaseProgress == false || i.IsOfficer == true && i.InChaseProgress == false)
                                    {
                                        Found = true;
                                    }
                                }

                                if (Found == false)
                                {
                                    MsgPly("^9 There are no Officers/Cadet can be found!", MSO.UCID);
                                }
                                if (Found == true)
                                {
                                    MsgAll("^9 Backup Request: ^7" + Conn.PlayerName + " (" + Conn.Username + ")");
                                    MsgAll("^9 Suspect Name: ^7" + ChaseCon.PlayerName + " (" + ChaseCon.Username + ")");
                                    MsgAll("^9 Suspect Condition: ^7" + Conn.ChaseCondition);
                                    MsgAll("^9 Suspect Location: ^7" + ChaseCon.Location);
                                }
                            }
                        }
                        else
                        {
                            MsgPly("^9 You must be in vehicle before you access this command!", MSO.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^9 You must be duty before you can access the cmnd!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^9 Not Authorized", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("cc", "cc")]
        public void copchat(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (Conn.CanBeOfficer == 1 && Conn.IsOfficer == true || Conn.CanBeCadet == 1 && Conn.IsCadet == true)
            {
                if (StrMsg.Length > 1)
                {
                    string MsgCC = Msg.Remove(0, 4);

                    foreach (clsConnection u in Connections)
                    {
                        if (u.IsOfficer == true && u.CanBeOfficer == 1 || u.IsCadet == true && u.CanBeCadet == 1)
                        {
                            MsgPly("^9 Cop Chat: " + Conn.PlayerName + " (" + Conn.Username + ")", u.UniqueID);
                            MsgPly("^9 Msg: " + MsgCC, u.UniqueID);
                        }
                    }

                }
                else
                {
                    if (StrMsg.Length == 1)
                    {
                        MsgPly("^9 Using cop chat ^2!cc <message>", MSO.UCID);
                    }
                    else
                    {
                        MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                    }
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        #endregion

        #region ' Tow Truck Command '

        [Command("accepttow", "acceptow <usrname>")]
        public void acceptrequest(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsTowTruck == true && Conn.CanBeTowTruck == 1)
                {
                    bool Found = false;
                    byte Accepted = 0;
                    string Username = Msg.Remove(0, 11);

                    foreach (clsConnection i in Connections)
                    {
                        if (i.Username == Username)
                        {
                            if (i.IsBeingTowed == false)
                            {
                                if (i.CalledRequest == true)
                                {
                                    Found = true;
                                    Accepted = 1;
                                    MsgPly("^9 " + Conn.PlayerName + " accepted your request!", i.UniqueID);
                                    i.CallAccepted = true;
                                    i.CalledRequest = false;
                                }
                            }
                            else
                            {
                                MsgPly("^9 The User is being Towed.", MSO.UCID);
                            }
                        }
                    }

                    if (Accepted == 0)
                    {
                        MsgPly("^9 No Call Request from this user or Being Accepted", MSO.UCID);
                    }
                    else if (Accepted == 1)
                    {
                        MsgPly("^9 Tow Request is now Accepted.", MSO.UCID);
                    }

                    if (Found == false)
                    {
                        MsgPly("^9 " + Username + " wasn't found or offline", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("starttow", "starttow")]
        public void starttow(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length == 1)
            {
                if (Conn.IsTowTruck == true && Conn.CanBeTowTruck == 1)
                {
                    if (Conn.InGame == 1)
                    {
                        if (Conn.InTowProgress == false)
                        {
                            try
                            {
                                int LowestDistance = 150;
                                int Distance = 0;
                                byte TowUCID = 0;
                                byte TowIndex = 0;
                                #region ' Start Tow '
                                if (Conn.UniqueID == MSO.UCID)
                                {
                                    #region ' Instance Tow '
                                    for (int i = 0; i < Connections.Count; i++)
                                    {
                                        if (Connections[i].PlayerID != 0)
                                        {
                                            Distance = ((int)Math.Sqrt(Math.Pow(Connections[i].CompCar.X - Conn.CompCar.X, 2) + Math.Pow(Connections[i].CompCar.Y - Conn.CompCar.Y, 2)) / 65536);
                                            Connections[i].DistanceFromTow = Distance;
                                        }
                                    }
                                    for (int i = 0; i < Connections.Count; i++)
                                    {
                                        if (Connections[i].PlayerID != 0)
                                        {
                                            if ((Connections[i].DistanceFromTow < LowestDistance) && (Connections[i].DistanceFromTow > 0) && (Connections[i].PlayerName != Conn.PlayerName) && (Connections[i].IsTowTruck == false))
                                            {
                                                LowestDistance = Connections[i].DistanceFromTow;

                                                TowUCID = Connections[i].UniqueID;
                                                TowIndex = (byte)i;
                                            }
                                        }
                                    }
                                    #endregion

                                    if ((LowestDistance < 150) && (Connections[GetConnIdx(TowUCID)].DistanceFromTow > 0))
                                    {
                                        if (Connections[TowIndex].CallAccepted == true || Connections[TowIndex].CalledRequest == true)
                                        {
                                            Conn.Towee = TowUCID;
                                            Connections[TowIndex].IsBeingTowed = true;
                                            Conn.InTowProgress = true;

                                            MsgAll("^9 " + Conn.PlayerName + " started a In Tow Progress!");
                                            MsgAll("^9 Tow Request: " + Connections[TowIndex].PlayerName);
                                            Connections[TowIndex].CallAccepted = false;
                                            Connections[TowIndex].CalledRequest = false;
                                        }
                                        else
                                        {
                                            MsgPly("^9 No Call Requested/Accepted on this user!", MSO.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^9 No Tow Request found in 50 meters!", MSO.UCID);
                                    }
                                }
                                #endregion
                            }
                            catch
                            {
                                MsgPly("^9 Engage Error.", MSO.UCID);
                            }
                        }
                        else
                        {
                            MsgPly("^9 You are already in Tow in Progress!", MSO.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^9 You must be in vehicle before you access this command!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("stoptow", "stoptow")]
        public void stoptow(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            var TowCon = Connections[GetConnIdx(Connections[GetConnIdx(MSO.UCID)].Towee)];
            if (StrMsg.Length == 1)
            {
                if (Conn.IsTowTruck == true && Conn.CanBeTowTruck == 1)
                {
                    if (Conn.InGame == 1)
                    {
                        if (Conn.InTowProgress == true)
                        {
                            MsgAll("^9 " + Conn.PlayerName + " stopped towing " + TowCon.PlayerName + "!");
                            MsgAll("^9 earned ^2$500 ");
                            TowCon.IsBeingTowed = false;
                            Conn.Towee = -1;
                            Conn.Cash += 500;
                            Conn.InTowProgress = false;
                            CautionSirenShutOff();
                        }
                        else
                        {
                            MsgPly("^9 Your not in Tow in Progress!", MSO.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^9 You must be in vehicle before you access this command!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        #endregion

        #endregion

        #region ' Admin/Moderator Command '

        [Command("addofficer", "addofficer <username>")]
        public void addofficer(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "skywatcher122")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 12);
                    #region ' Online '
                    foreach (clsConnection l in Connections)
                    {
                        if (l.Username == Username)
                        {
                            Found = true;
                            if (l.CanBeOfficer == 0)
                            {
                                MsgAll("^9 " + l.PlayerName + " can be now an Officer!");
                                MsgPly("^9 To get in duty use the " + OfficerTag + " ^7to get duty!", l.UniqueID);
                                //mBox("> " + Conn.PlayerName + " added " + l.PlayerName + " in Police Officer Force!");
                                l.CanBeOfficer = 1;
                            }
                            else if (l.CanBeOfficer == 1)
                            {
                                MsgPly("^9 " + l.PlayerName + " is already an Officer!", MSO.UCID);
                            }

                            // Remove Cadetory
                            if (l.CanBeCadet == 0 || l.CanBeCadet == 1)
                            {
                                l.CanBeCadet = 2;
                            }
                        }
                    }
                    #endregion

                    #region ' Offline '

                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (PlayerName.Contains("^0"))
                            {
                                PlayerName = PlayerName.Replace("^0", "");
                            }
                            if (PlayerName.Contains("^1"))
                            {
                                PlayerName = PlayerName.Replace("^1", "");
                            }
                            if (PlayerName.Contains("^2"))
                            {
                                PlayerName = PlayerName.Replace("^2", "");
                            }
                            if (PlayerName.Contains("^3"))
                            {
                                PlayerName = PlayerName.Replace("^3", "");
                            }
                            if (PlayerName.Contains("^4"))
                            {
                                PlayerName = PlayerName.Replace("^4", "");
                            }
                            if (PlayerName.Contains("^5"))
                            {
                                PlayerName = PlayerName.Replace("^5", "");
                            }
                            if (PlayerName.Contains("^6"))
                            {
                                PlayerName = PlayerName.Replace("^6", "");
                            }
                            if (PlayerName.Contains("^7"))
                            {
                                PlayerName = PlayerName.Replace("^7", "");
                            }
                            if (PlayerName.Contains("^8"))
                            {
                                PlayerName = PlayerName.Replace("^8", "");
                            }
                            #endregion

                            bool SaveUser = false;
                            if (CanBeOfficer == 0)
                            {
                                SaveUser = true;
                                CanBeOfficer = 1;
                                MsgAll("^9 " + PlayerName + " can be now an Officer!");
                                //AdmBox("> " + Conn.PlayerName + " added " + PlayerName + " in Police Officer Force!");
                            }
                            else if (CanBeOfficer == 1)
                            {
                                MsgPly("^9 " + PlayerName + " is already an Officer!", MSO.UCID);
                            }

                            // Remove Cadetory
                            if (CanBeCadet == 1 || CanBeCadet == 0)
                            {
                                SaveUser = true;
                                CanBeCadet = 2;
                            }

                            #region ' Save User '
                            if (SaveUser == true)
                            {
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);
                            }
                            #endregion
                        }
                        else
                        {
                            MsgPly("^9 " + Username + " wasn't found on database", MSO.UCID);
                        }
                    }

                    #endregion
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                    //AdmBox("> " + Conn.PlayerName + " (" + Conn.Username + ") tried to access the Add Officer Command!");
                    MsgAll("^9 " + Conn.PlayerName + " tried to use Add Officer Command!");
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("removecop", "removecop <username>")]
        public void removecop(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "skywatcher122")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 11);

                    #region ' Online '
                    foreach (clsConnection l in Connections)
                    {
                        if (l.Username == Username)
                        {
                            Found = true;
                            var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(l.UniqueID)].Chasee)];
                            if (l.IsOfficer == true)
                            {
                                #region ' InChase Progress '
                                if (l.InChaseProgress == true)
                                {
                                    #region ' In Chase Progress '
                                    if (ChaseCon.CopInChase > 1)
                                    {
                                        if (l.JoinedChase == true)
                                        {
                                            l.JoinedChase = false;
                                        }
                                        l.ChaseCondition = 0;
                                        l.Busted = false;
                                        l.BustedTimer = 0;
                                        l.BumpButton = 0;
                                        l.Chasee = -1;
                                        ChaseCon.CopInChase -= 1;

                                        #region ' Connection List '
                                        if (ChaseCon.CopInChase == 1)
                                        {
                                            foreach (clsConnection Con in Connections)
                                            {
                                                if (Con.Chasee == ChaseCon.UniqueID)
                                                {
                                                    if (Con.JoinedChase == true)
                                                    {
                                                        Con.JoinedChase = false;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        MsgAll("^9 " + l.PlayerName + " sighting lost " + ChaseCon.PlayerName + "!");
                                        MsgAll("^7 Total Cops In Chase: " + ChaseCon.CopInChase);

                                    }
                                    else if (ChaseCon.CopInChase == 1)
                                    {
                                        MsgAll("^9 " + l.PlayerName + " lost " + ChaseCon.PlayerName + "!");
                                        MsgAll("^7Suspect Runs away from being chased!");
                                        AddChaseLimit -= 1;
                                        l.AutoBumpTimer = 0;
                                        l.BumpButton = 0;
                                        l.BustedTimer = 0;
                                        l.Chasee = -1;
                                        l.Busted = false;
                                        ChaseCon.PullOvrMsg = 0;
                                        ChaseCon.CopInChase = 0;
                                        ChaseCon.IsSuspect = false;
                                        ChaseCon.ChaseCondition = 0;
                                        l.ChaseCondition = 0;
                                        CopSirenShutOff();
                                    }
                                    #endregion

                                    #region ' Remove Cop Panel '

                                    DeleteBTN(15, l.UniqueID);
                                    DeleteBTN(16, l.UniqueID);
                                    DeleteBTN(17, l.UniqueID);
                                    DeleteBTN(18, l.UniqueID);
                                    DeleteBTN(19, l.UniqueID);
                                    DeleteBTN(20, l.UniqueID);
                                    DeleteBTN(21, l.UniqueID);
                                    DeleteBTN(22, l.UniqueID);

                                    #endregion

                                    #region ' Restore some BTN '
                                    if (l.InGameIntrfc == 0 && l.DisplaysOpen == true)
                                    {
                                        if (l.InShop == true)
                                        {
                                            if (l.CurrentCar == "UF1" || l.CurrentCar == "XFG" || l.CurrentCar == "XRG" || l.CurrentCar == "LX4" || l.CurrentCar == "LX6" || l.CurrentCar == "RB4" || l.CurrentCar == "FXO" || l.CurrentCar == "XRT" || l.CurrentCar == "VWS" || l.CurrentCar == "RAC" || l.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, l.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, l.UniqueID, 2, false);
                                            }
                                        }
                                        if (l.InStore == true)
                                        {
                                            if (l.CurrentCar == "UF1" || l.CurrentCar == "XFG" || l.CurrentCar == "XRG" || l.CurrentCar == "LX4" || l.CurrentCar == "LX6" || l.CurrentCar == "RB4" || l.CurrentCar == "FXO" || l.CurrentCar == "XRT" || l.CurrentCar == "VWS" || l.CurrentCar == "RAC" || l.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, l.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, l.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    #endregion

                                    l.InChaseProgress = false;

                                    MsgAll("^9 " + l.LastName + " was forced to ^1OFF-DUTY ^7as a Officer!");
                                    TotalOfficers -= 1;
                                    l.LastName = "";
                                }
                                else
                                {
                                    #region ' Remove Cop Panel '

                                    DeleteBTN(15, l.UniqueID);
                                    DeleteBTN(16, l.UniqueID);
                                    DeleteBTN(17, l.UniqueID);
                                    DeleteBTN(18, l.UniqueID);
                                    DeleteBTN(19, l.UniqueID);
                                    DeleteBTN(20, l.UniqueID);
                                    DeleteBTN(21, l.UniqueID);
                                    DeleteBTN(22, l.UniqueID);

                                    #endregion

                                    #region ' Restore some BTN '
                                    if (l.InGameIntrfc == 0 && l.DisplaysOpen == true)
                                    {
                                        if (l.InShop == true)
                                        {
                                            if (l.CurrentCar == "UF1" || l.CurrentCar == "XFG" || l.CurrentCar == "XRG" || l.CurrentCar == "LX4" || l.CurrentCar == "LX6" || l.CurrentCar == "RB4" || l.CurrentCar == "FXO" || l.CurrentCar == "XRT" || l.CurrentCar == "VWS" || l.CurrentCar == "RAC" || l.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, l.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, l.UniqueID, 2, false);
                                            }
                                        }
                                        if (l.InStore == true)
                                        {
                                            if (l.CurrentCar == "UF1" || l.CurrentCar == "XFG" || l.CurrentCar == "XRG" || l.CurrentCar == "LX4" || l.CurrentCar == "LX6" || l.CurrentCar == "RB4" || l.CurrentCar == "FXO" || l.CurrentCar == "XRT" || l.CurrentCar == "VWS" || l.CurrentCar == "RAC" || l.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, l.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, l.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    #endregion

                                    MsgAll("^9 " + l.LastName + " was forced to ^1OFF-DUTY ^7as a Officer!");
                                    TotalOfficers -= 1;
                                    l.LastName = "";
                                }
                                #endregion

                                #region ' Busted Remove '
                                if (l.InFineMenu == true)
                                {
                                    MsgAll("^9 " + l.PlayerName + " released " + ChaseCon.PlayerName + "!");

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
                                    DeleteBTN(30, l.UniqueID);
                                    DeleteBTN(31, l.UniqueID);
                                    DeleteBTN(32, l.UniqueID);
                                    DeleteBTN(33, l.UniqueID);
                                    DeleteBTN(34, l.UniqueID);
                                    DeleteBTN(35, l.UniqueID);
                                    DeleteBTN(36, l.UniqueID);
                                    DeleteBTN(37, l.UniqueID);
                                    DeleteBTN(38, l.UniqueID);
                                    DeleteBTN(39, l.UniqueID);
                                    DeleteBTN(40, l.UniqueID);
                                    #endregion

                                    if (l.InFineMenu == true)
                                    {
                                        l.InFineMenu = false;
                                    }

                                    l.Busted = false;
                                }
                                #endregion

                                l.IsOfficer = false;

                                if (l.CanBeOfficer == 1)
                                {
                                    MsgAll("^9 " + l.PlayerName + " is now removed as Officer!");
                                    //AdmBox("> " + Conn.PlayerName + " removed " + l.PlayerName + " in Police Officer Force!");
                                    l.CanBeOfficer = 0;
                                }
                            }
                            else if (l.IsOfficer == false)
                            {
                                if (l.CanBeOfficer == 1)
                                {
                                    MsgAll("^9 " + l.PlayerName + " is now removed as Officer!");
                                    //AdmBox("> " + Conn.PlayerName + " removed " + l.PlayerName + " in Police Officer Force!");
                                    l.CanBeOfficer = 0;
                                }
                            }

                            if (l.CanBeOfficer == 0)
                            {
                                MsgPly("^9 " + l.PlayerName + " is not an Officer!", MSO.UCID);
                            }
                        }
                    }
                    #endregion

                    #region ' Offline '
                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (PlayerName.Contains("^0"))
                            {
                                PlayerName = PlayerName.Replace("^0", "");
                            }
                            if (PlayerName.Contains("^1"))
                            {
                                PlayerName = PlayerName.Replace("^1", "");
                            }
                            if (PlayerName.Contains("^2"))
                            {
                                PlayerName = PlayerName.Replace("^2", "");
                            }
                            if (PlayerName.Contains("^3"))
                            {
                                PlayerName = PlayerName.Replace("^3", "");
                            }
                            if (PlayerName.Contains("^4"))
                            {
                                PlayerName = PlayerName.Replace("^4", "");
                            }
                            if (PlayerName.Contains("^5"))
                            {
                                PlayerName = PlayerName.Replace("^5", "");
                            }
                            if (PlayerName.Contains("^6"))
                            {
                                PlayerName = PlayerName.Replace("^6", "");
                            }
                            if (PlayerName.Contains("^7"))
                            {
                                PlayerName = PlayerName.Replace("^7", "");
                            }
                            if (PlayerName.Contains("^8"))
                            {
                                PlayerName = PlayerName.Replace("^8", "");
                            }
                            #endregion
                            bool Complete = true;
                            // Your Command here!
                            if (CanBeOfficer == 1)
                            {
                                Complete = true;
                                CanBeOfficer = 0;
                                MsgAll("^9 " + PlayerName + " is now removed as Officer!");
                                //AdmBox("> " + Conn.PlayerName + " removed " + PlayerName + " in Police Officer Force!");
                            }
                            else if (CanBeOfficer == 0)
                            {
                                MsgPly("^9 " + PlayerName + " is not an Officer!", MSO.UCID);
                            }

                            #region ' Save User '
                            if (Complete == true)
                            {
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);
                            }
                            #endregion
                        }
                        else
                        {
                            MsgPly("^9 " + Username + " wasn't found on database", MSO.UCID);
                        }
                    }
                    #endregion
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                    //AdmBox("> " + Conn.PlayerName + " (" + Conn.Username + ") tried to access the Remove Cop Command!");
                    MsgAll("^9 " + Conn.PlayerName + " tried to use Remove Cop Command!");
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("addcadet", "addcadet <username>")]
        public void addcadet(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "skywatcher122")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 10);

                    #region ' Online Adding '

                    foreach (clsConnection l in Connections)
                    {
                        if (l.Username == Username)
                        {
                            Found = true;

                            if (l.CanBeOfficer == 1)
                            {
                                MsgPly("^9 " + l.PlayerName + " is already a Officer", MSO.UCID);
                            }
                            else if (l.CanBeOfficer == 0)
                            {
                                if (l.CanBeCadet == 0 || l.CanBeCadet == 2 || l.CanBeCadet == 3)
                                {
                                    MsgAll("^9 " + l.PlayerName + " is now added as Cadet!");
                                    //AdmBox("> " + Conn.PlayerName + " added " + l.PlayerName + " in Police Cadet Force!");
                                    l.CanBeCadet = 1;
                                }
                            }
                        }
                    }

                    #endregion

                    #region ' Offline Adding '

                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (PlayerName.Contains("^0"))
                            {
                                PlayerName = PlayerName.Replace("^0", "");
                            }
                            if (PlayerName.Contains("^1"))
                            {
                                PlayerName = PlayerName.Replace("^1", "");
                            }
                            if (PlayerName.Contains("^2"))
                            {
                                PlayerName = PlayerName.Replace("^2", "");
                            }
                            if (PlayerName.Contains("^3"))
                            {
                                PlayerName = PlayerName.Replace("^3", "");
                            }
                            if (PlayerName.Contains("^4"))
                            {
                                PlayerName = PlayerName.Replace("^4", "");
                            }
                            if (PlayerName.Contains("^5"))
                            {
                                PlayerName = PlayerName.Replace("^5", "");
                            }
                            if (PlayerName.Contains("^6"))
                            {
                                PlayerName = PlayerName.Replace("^6", "");
                            }
                            if (PlayerName.Contains("^7"))
                            {
                                PlayerName = PlayerName.Replace("^7", "");
                            }
                            if (PlayerName.Contains("^8"))
                            {
                                PlayerName = PlayerName.Replace("^8", "");
                            }
                            #endregion

                            bool Complete = false;
                            // Your Command here!

                            if (CanBeOfficer == 0)
                            {
                                if (CanBeCadet == 0 || CanBeCadet == 2 || CanBeCadet == 3)
                                {
                                    Complete = true;
                                    MsgAll("^9 " + PlayerName + " can be now a Cadet!");
                                    //AdmBox("> " + Conn.PlayerName + " added " + PlayerName + " in Police Cadet Force!");
                                    CanBeCadet = 1;
                                }
                            }
                            else if (CanBeOfficer == 1)
                            {
                                MsgPly("^9 " + PlayerName + " is already a Officer!", MSO.UCID);
                            }

                            if (CanBeCadet == 1)
                            {
                                MsgPly("^9 " + PlayerName + " is already a Cadet!", MSO.UCID);
                            }

                            #region ' Save User '
                            if (Complete == true)
                            {
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);
                            }
                            #endregion
                        }
                        else
                        {
                            MsgPly("^9 " + Username + " wasn't found on database", MSO.UCID);
                        }
                    }

                    #endregion
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                    //AdmBox("> " + Conn.PlayerName + " (" + Conn.Username + ") tried to access the Add Cadet Command!");
                    MsgAll("^9 " + Conn.PlayerName + " tried to use Add Cadet Command!");
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("remcadet", "remcadet <username>")]
        public void removecadet(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "skywatcher122")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 10);

                    #region ' Online Removing '

                    foreach (clsConnection l in Connections)
                    {
                        if (l.Username == Username)
                        {
                            Found = true;
                            var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(l.UniqueID)].Chasee)];
                            if (l.IsCadet == true)
                            {
                                #region ' InChase Progress '
                                if (l.InChaseProgress == true)
                                {
                                    #region ' In Chase Progress '
                                    if (ChaseCon.CopInChase > 1)
                                    {
                                        if (l.JoinedChase == true)
                                        {
                                            l.JoinedChase = false;
                                        }
                                        l.ChaseCondition = 0;
                                        l.Busted = false;
                                        l.BustedTimer = 0;
                                        l.BumpButton = 0;
                                        l.Chasee = -1;
                                        ChaseCon.CopInChase -= 1;

                                        #region ' Connection List '
                                        if (ChaseCon.CopInChase == 1)
                                        {
                                            foreach (clsConnection Con in Connections)
                                            {
                                                if (Con.Chasee == ChaseCon.UniqueID)
                                                {
                                                    if (Con.JoinedChase == true)
                                                    {
                                                        Con.JoinedChase = false;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        MsgAll("^9 " + l.PlayerName + " sighting lost " + ChaseCon.PlayerName + "!");
                                        MsgAll("^7 Total Cops In Chase: " + ChaseCon.CopInChase);

                                    }
                                    else if (ChaseCon.CopInChase == 1)
                                    {
                                        MsgAll("^9 " + l.PlayerName + " lost " + ChaseCon.PlayerName + "!");
                                        MsgAll("^7Suspect Runs away from being chased!");
                                        AddChaseLimit -= 1;
                                        l.AutoBumpTimer = 0;
                                        l.BumpButton = 0;
                                        l.BustedTimer = 0;
                                        l.Chasee = -1;
                                        l.Busted = false;
                                        ChaseCon.PullOvrMsg = 0;
                                        ChaseCon.CopInChase = 0;
                                        ChaseCon.IsSuspect = false;
                                        ChaseCon.ChaseCondition = 0;
                                        l.ChaseCondition = 0;
                                        CopSirenShutOff();
                                    }
                                    #endregion

                                    #region ' Remove Cop Panel '

                                    DeleteBTN(15, l.UniqueID);
                                    DeleteBTN(16, l.UniqueID);
                                    DeleteBTN(17, l.UniqueID);
                                    DeleteBTN(18, l.UniqueID);
                                    DeleteBTN(19, l.UniqueID);
                                    DeleteBTN(20, l.UniqueID);
                                    DeleteBTN(21, l.UniqueID);
                                    DeleteBTN(22, l.UniqueID);

                                    #endregion

                                    #region ' Restore some BTN '
                                    if (l.InGameIntrfc == 0 && l.DisplaysOpen == true)
                                    {
                                        if (l.InShop == true)
                                        {
                                            if (l.CurrentCar == "UF1" || l.CurrentCar == "XFG" || l.CurrentCar == "XRG" || l.CurrentCar == "LX4" || l.CurrentCar == "LX6" || l.CurrentCar == "RB4" || l.CurrentCar == "FXO" || l.CurrentCar == "XRT" || l.CurrentCar == "VWS" || l.CurrentCar == "RAC" || l.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, l.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, l.UniqueID, 2, false);
                                            }
                                        }
                                        if (l.InStore == true)
                                        {
                                            if (l.CurrentCar == "UF1" || l.CurrentCar == "XFG" || l.CurrentCar == "XRG" || l.CurrentCar == "LX4" || l.CurrentCar == "LX6" || l.CurrentCar == "RB4" || l.CurrentCar == "FXO" || l.CurrentCar == "XRT" || l.CurrentCar == "VWS" || l.CurrentCar == "RAC" || l.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, l.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, l.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    #endregion

                                    l.InChaseProgress = false;

                                    MsgAll("^9 " + l.LastName + " was forced to ^1OFF-DUTY ^7as a Cadet!");
                                    TotalOfficers -= 1;
                                    l.LastName = "";
                                }
                                else
                                {
                                    #region ' Remove Cop Panel '

                                    DeleteBTN(15, l.UniqueID);
                                    DeleteBTN(16, l.UniqueID);
                                    DeleteBTN(17, l.UniqueID);
                                    DeleteBTN(18, l.UniqueID);
                                    DeleteBTN(19, l.UniqueID);
                                    DeleteBTN(20, l.UniqueID);
                                    DeleteBTN(21, l.UniqueID);
                                    DeleteBTN(22, l.UniqueID);

                                    #endregion

                                    #region ' Restore some BTN '
                                    if (l.InGameIntrfc == 0 && l.DisplaysOpen == true)
                                    {
                                        if (l.InShop == true)
                                        {
                                            if (l.CurrentCar == "UF1" || l.CurrentCar == "XFG" || l.CurrentCar == "XRG" || l.CurrentCar == "LX4" || l.CurrentCar == "LX6" || l.CurrentCar == "RB4" || l.CurrentCar == "FXO" || l.CurrentCar == "XRT" || l.CurrentCar == "VWS" || l.CurrentCar == "RAC" || l.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, l.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, l.UniqueID, 2, false);
                                            }
                                        }
                                        if (l.InStore == true)
                                        {
                                            if (l.CurrentCar == "UF1" || l.CurrentCar == "XFG" || l.CurrentCar == "XRG" || l.CurrentCar == "LX4" || l.CurrentCar == "LX6" || l.CurrentCar == "RB4" || l.CurrentCar == "FXO" || l.CurrentCar == "XRT" || l.CurrentCar == "VWS" || l.CurrentCar == "RAC" || l.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, l.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, l.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    #endregion

                                    MsgAll("^9 " + l.LastName + " was forced to ^1OFF-DUTY ^7as a Cadet!");
                                    TotalOfficers -= 1;
                                    l.LastName = "";
                                }
                                #endregion

                                #region ' Busted Remove '
                                if (l.InFineMenu == true)
                                {
                                    MsgAll("^9 " + l.PlayerName + " released " + ChaseCon.PlayerName + "!");

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
                                    DeleteBTN(30, l.UniqueID);
                                    DeleteBTN(31, l.UniqueID);
                                    DeleteBTN(32, l.UniqueID);
                                    DeleteBTN(33, l.UniqueID);
                                    DeleteBTN(34, l.UniqueID);
                                    DeleteBTN(35, l.UniqueID);
                                    DeleteBTN(36, l.UniqueID);
                                    DeleteBTN(37, l.UniqueID);
                                    DeleteBTN(38, l.UniqueID);
                                    DeleteBTN(39, l.UniqueID);
                                    DeleteBTN(40, l.UniqueID);
                                    #endregion

                                    if (l.InFineMenu == true)
                                    {
                                        l.InFineMenu = false;
                                    }

                                    l.Busted = false;
                                }
                                #endregion

                                l.IsCadet = false;

                                if (l.CanBeCadet == 1)
                                {
                                    MsgAll("^9 " + l.PlayerName + " is now removed as Cadet!");

                                    l.CanBeCadet = 3;
                                }
                            }
                            else if (l.IsOfficer == false)
                            {
                                if (l.CanBeCadet == 1)
                                {
                                    MsgAll("^9 " + l.PlayerName + " is now removed as Officer!");

                                    l.CanBeCadet = 3;
                                }
                            }

                            if (l.CanBeCadet == 0 || l.CanBeCadet == 2 || l.CanBeCadet == 3)
                            {
                                MsgPly("^9 " + l.PlayerName + " is not an Cadet!", MSO.UCID);
                            }
                        }
                    }

                    #endregion

                    #region ' Offline Removing '

                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (PlayerName.Contains("^0"))
                            {
                                PlayerName = PlayerName.Replace("^0", "");
                            }
                            if (PlayerName.Contains("^1"))
                            {
                                PlayerName = PlayerName.Replace("^1", "");
                            }
                            if (PlayerName.Contains("^2"))
                            {
                                PlayerName = PlayerName.Replace("^2", "");
                            }
                            if (PlayerName.Contains("^3"))
                            {
                                PlayerName = PlayerName.Replace("^3", "");
                            }
                            if (PlayerName.Contains("^4"))
                            {
                                PlayerName = PlayerName.Replace("^4", "");
                            }
                            if (PlayerName.Contains("^5"))
                            {
                                PlayerName = PlayerName.Replace("^5", "");
                            }
                            if (PlayerName.Contains("^6"))
                            {
                                PlayerName = PlayerName.Replace("^6", "");
                            }
                            if (PlayerName.Contains("^7"))
                            {
                                PlayerName = PlayerName.Replace("^7", "");
                            }
                            if (PlayerName.Contains("^8"))
                            {
                                PlayerName = PlayerName.Replace("^8", "");
                            }
                            #endregion

                            bool Complete = false;
                            // Your Command here!
                            if (CanBeCadet == 1)
                            {
                                Complete = true;
                                MsgAll("^9 " + PlayerName + " is now removed as Cadet!");
                                //AdmBox("> " + Conn.PlayerName + " removed " + PlayerName + " in Police Cadet Force!");
                                CanBeCadet = 3;
                            }
                            else if (CanBeCadet == 0 || CanBeCadet == 2 || CanBeCadet == 3)
                            {
                                MsgPly("^9 " + PlayerName + " is not an Cadet!", MSO.UCID);
                            }



                            #region ' Save User '
                            if (Complete == true)
                            {
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);
                            }
                            #endregion
                        }
                        else
                        {
                            MsgPly("^9 " + Username + " wasn't found on database", MSO.UCID);
                        }
                    }

                    #endregion
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                    //AdmBox("> " + Conn.PlayerName + " (" + Conn.Username + ") tried to access the Remove Cadet Command!");
                    MsgAll("^9 " + Conn.PlayerName + " tried to use Remove Cadet Command!");
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("addtow", "addtow <username>")]
        public void addtow(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "skywatcher122")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 8);

                    #region ' Online Adding '

                    foreach (clsConnection l in Connections)
                    {
                        if (l.Username == Username)
                        {
                            Found = true;
                            if (l.CanBeTowTruck == 0)
                            {
                                l.CanBeTowTruck = 1;
                                MsgAll("^9 " + l.PlayerName + " can be now a Tow Truck!");

                            }
                            else
                            {
                                MsgPly("^9 " + l.PlayerName + " is already a Tow Truck", MSO.UCID);
                            }
                        }
                    }

                    #endregion

                    #region ' Offline Adding '

                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (PlayerName.Contains("^0"))
                            {
                                PlayerName = PlayerName.Replace("^0", "");
                            }
                            if (PlayerName.Contains("^1"))
                            {
                                PlayerName = PlayerName.Replace("^1", "");
                            }
                            if (PlayerName.Contains("^2"))
                            {
                                PlayerName = PlayerName.Replace("^2", "");
                            }
                            if (PlayerName.Contains("^3"))
                            {
                                PlayerName = PlayerName.Replace("^3", "");
                            }
                            if (PlayerName.Contains("^4"))
                            {
                                PlayerName = PlayerName.Replace("^4", "");
                            }
                            if (PlayerName.Contains("^5"))
                            {
                                PlayerName = PlayerName.Replace("^5", "");
                            }
                            if (PlayerName.Contains("^6"))
                            {
                                PlayerName = PlayerName.Replace("^6", "");
                            }
                            if (PlayerName.Contains("^7"))
                            {
                                PlayerName = PlayerName.Replace("^7", "");
                            }
                            if (PlayerName.Contains("^8"))
                            {
                                PlayerName = PlayerName.Replace("^8", "");
                            }
                            #endregion

                            bool Complete = false;
                            // Your Command here!
                            if (CanBeTowTruck == 0)
                            {
                                Complete = true;
                                CanBeTowTruck = 1;
                                MsgAll("^9 " + PlayerName + " can be now a TowTruck!");

                            }
                            else
                            {
                                MsgPly("^9 " + PlayerName + " is already a Tow Truck", MSO.UCID);
                            }

                            #region ' Save User '
                            if (Complete == true)
                            {
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);
                            }
                            #endregion
                        }
                        else
                        {
                            MsgPly("^9 " + Username + " wasn't found on database", MSO.UCID);
                        }
                    }

                    #endregion
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                    //AdmBox("> " + Conn.PlayerName + " (" + Conn.Username + ") tried to access the Add TowTruck Command!");
                    MsgAll("^9 " + Conn.PlayerName + " tried to use Add TowTruck Command!");
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("remtow", "remtow <username>")]
        public void remtow(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "skywatcher122")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 8);

                    #region ' Online Removing '

                    foreach (clsConnection l in Connections)
                    {
                        if (l.Username == Username)
                        {
                            Found = true;
                            var TowCon = Connections[GetConnIdx(Connections[GetConnIdx(l.UniqueID)].Towee)];
                            if (l.IsTowTruck == true)
                            {
                                if (l.InTowProgress == true)
                                {
                                    if (TowCon.IsBeingTowed == true)
                                    {
                                        TowCon.IsBeingTowed = false;
                                    }
                                    l.Towee = -1;
                                    l.InTowProgress = false;
                                    CautionSirenShutOff();
                                }

                                l.IsTowTruck = false;
                                MsgAll("^9 " + l.PlayerName + " was forced ^1OFF-DUTY ^7as TowTruck!");
                                if (l.CanBeTowTruck == 1)
                                {
                                    l.CanBeTowTruck = 0;
                                    MsgAll("^9 " + l.PlayerName + " is now removed as Tow Truck!");
                                    //AdmBox("> " + Conn.PlayerName + " removed " + l.PlayerName + " as Tow Truck!");
                                }
                            }
                            else
                            {
                                if (l.CanBeTowTruck == 1)
                                {
                                    l.CanBeTowTruck = 0;
                                    MsgAll("^9 " + l.PlayerName + " is now removed as Tow Truck!");
                                    //AdmBox("> " + Conn.PlayerName + " removed " + l.PlayerName + " as Tow Truck!");
                                }
                                else
                                {
                                    MsgPly("^9 " + l.PlayerName + " is not a Tow Truck", MSO.UCID);
                                }
                            }
                        }
                    }

                    #endregion

                    #region ' Offline Removing '

                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (PlayerName.Contains("^0"))
                            {
                                PlayerName = PlayerName.Replace("^0", "");
                            }
                            if (PlayerName.Contains("^1"))
                            {
                                PlayerName = PlayerName.Replace("^1", "");
                            }
                            if (PlayerName.Contains("^2"))
                            {
                                PlayerName = PlayerName.Replace("^2", "");
                            }
                            if (PlayerName.Contains("^3"))
                            {
                                PlayerName = PlayerName.Replace("^3", "");
                            }
                            if (PlayerName.Contains("^4"))
                            {
                                PlayerName = PlayerName.Replace("^4", "");
                            }
                            if (PlayerName.Contains("^5"))
                            {
                                PlayerName = PlayerName.Replace("^5", "");
                            }
                            if (PlayerName.Contains("^6"))
                            {
                                PlayerName = PlayerName.Replace("^6", "");
                            }
                            if (PlayerName.Contains("^7"))
                            {
                                PlayerName = PlayerName.Replace("^7", "");
                            }
                            if (PlayerName.Contains("^8"))
                            {
                                PlayerName = PlayerName.Replace("^8", "");
                            }
                            #endregion

                            bool Complete = false;
                            // Your Command here!
                            if (CanBeTowTruck == 1)
                            {
                                Complete = true;
                                CanBeTowTruck = 0;
                                MsgAll("^9 " + PlayerName + " is now removed as Tow Truck!");
                                //AdmBox("> " + Conn.PlayerName + " removed " + PlayerName + " as Tow Truck!");
                            }
                            else if (CanBeTowTruck == 0)
                            {
                                MsgPly("^9 " + PlayerName + " is not a Tow Truck", MSO.UCID);
                            }

                            #region ' Save User '
                            if (Complete == true)
                            {
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);
                            }
                            #endregion
                        }
                        else
                        {
                            MsgPly("^9 " + Username + " wasn't found on database", MSO.UCID);
                        }
                    }

                    #endregion
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);

                    MsgAll("^9 " + Conn.PlayerName + " tried to use Remove TowTruck Command!");
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("addmember", "addmember <username>")]
        public void addmember(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "skywatcher122")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 11);

                    #region ' Online Removing '

                    foreach (clsConnection l in Connections)
                    {
                        if (l.Username == Username)
                        {
                            Found = true;
                            if (FileInfo.GetUserAdmin(Username) == 1)
                            {
                                MsgPly("^9 " + l.PlayerName + " is already a Admin!", MSO.UCID);
                            }
                            else
                            {
                                if (l.IsModerator == 0)
                                {
                                    MsgAll("^9 " + l.PlayerName + " can be now a Moderator!");
                                    //("> " + Conn.PlayerName + " added " + l.PlayerName + " as Moderator!");
                                    l.IsModerator = 1;
                                }
                                else
                                {
                                    MsgPly("^9 " + l.PlayerName + " is already a Moderator!", MSO.UCID);
                                }
                            }
                        }
                    }

                    #endregion

                    #region ' Offline Removing '

                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (PlayerName.Contains("^0"))
                            {
                                PlayerName = PlayerName.Replace("^0", "");
                            }
                            if (PlayerName.Contains("^1"))
                            {
                                PlayerName = PlayerName.Replace("^1", "");
                            }
                            if (PlayerName.Contains("^2"))
                            {
                                PlayerName = PlayerName.Replace("^2", "");
                            }
                            if (PlayerName.Contains("^3"))
                            {
                                PlayerName = PlayerName.Replace("^3", "");
                            }
                            if (PlayerName.Contains("^4"))
                            {
                                PlayerName = PlayerName.Replace("^4", "");
                            }
                            if (PlayerName.Contains("^5"))
                            {
                                PlayerName = PlayerName.Replace("^5", "");
                            }
                            if (PlayerName.Contains("^6"))
                            {
                                PlayerName = PlayerName.Replace("^6", "");
                            }
                            if (PlayerName.Contains("^7"))
                            {
                                PlayerName = PlayerName.Replace("^7", "");
                            }
                            if (PlayerName.Contains("^8"))
                            {
                                PlayerName = PlayerName.Replace("^8", "");
                            }
                            #endregion

                            bool Complete = false;
                            // Your Command here!
                            if (FileInfo.GetUserAdmin(Username) == 1)
                            {
                                MsgPly("^9 " + PlayerName + " is already a Admin!", MSO.UCID);
                            }
                            else
                            {
                                if (IsModerator == 0)
                                {
                                    Complete = true;
                                    IsModerator = 1;
                                    MsgAll("^9 " + PlayerName + " is now added as Moderator!");

                                }
                                else if (IsModerator == 1)
                                {
                                    MsgPly("^9 " + PlayerName + " is already a Moderator!", MSO.UCID);
                                }
                            }
                            #region ' Save User '
                            if (Complete == true)
                            {
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);
                            }
                            #endregion
                        }
                        else
                        {
                            MsgPly("^9 " + Username + " wasn't found on database", MSO.UCID);
                        }
                    }

                    #endregion
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                    //("> " + Conn.PlayerName + " (" + Conn.Username + ") tried to access the Add Moderator Command!");
                    MsgAll("^9 " + Conn.PlayerName + " tried to use Add Moderator Command!");
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("remmember", "remmember <username>")]
        public void remmember(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "skywatcher122")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 11);

                    #region ' Online Removing '

                    foreach (clsConnection l in Connections)
                    {
                        if (l.Username == Username)
                        {
                            Found = true;
                            if (FileInfo.GetUserAdmin(Username) == 1)
                            {
                                MsgPly("^9 " + l.PlayerName + " is already a Admin!", MSO.UCID);
                            }
                            else
                            {
                                if (l.InModerationMenu > 0)
                                {
                                    #region ' Close Region LOL '
                                    DeleteBTN(30, l.UniqueID);
                                    DeleteBTN(31, l.UniqueID);
                                    DeleteBTN(32, l.UniqueID);
                                    DeleteBTN(33, l.UniqueID);
                                    DeleteBTN(34, l.UniqueID);
                                    DeleteBTN(35, l.UniqueID);
                                    DeleteBTN(36, l.UniqueID);
                                    DeleteBTN(37, l.UniqueID);
                                    DeleteBTN(38, l.UniqueID);
                                    DeleteBTN(39, l.UniqueID);
                                    DeleteBTN(40, l.UniqueID);
                                    DeleteBTN(41, l.UniqueID);
                                    DeleteBTN(42, l.UniqueID);
                                    DeleteBTN(43, l.UniqueID);
                                    #endregion

                                    l.ModerationWarn = 0;
                                    l.ModUsername = "";
                                    l.ModReason = "";
                                    l.ModReasonSet = false;
                                    l.InModerationMenu = 0;


                                    if (l.IsModerator == 1)
                                    {
                                        MsgAll("^9 " + l.PlayerName + " is now removed as Moderator!");
                                        l.IsModerator = 0;
                                    }
                                    else
                                    {
                                        MsgPly("^9 " + l.PlayerName + " is not a Moderator!", MSO.UCID);
                                    }
                                }
                                else
                                {
                                    if (l.IsModerator == 1)
                                    {
                                        MsgAll("^9 " + l.PlayerName + " is now removed as Moderator!");
                                        l.IsModerator = 0;
                                    }
                                    else
                                    {
                                        MsgPly("^9 " + l.PlayerName + " is not a Moderator!", MSO.UCID);
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    #region ' Offline Removing '

                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (PlayerName.Contains("^0"))
                            {
                                PlayerName = PlayerName.Replace("^0", "");
                            }
                            if (PlayerName.Contains("^1"))
                            {
                                PlayerName = PlayerName.Replace("^1", "");
                            }
                            if (PlayerName.Contains("^2"))
                            {
                                PlayerName = PlayerName.Replace("^2", "");
                            }
                            if (PlayerName.Contains("^3"))
                            {
                                PlayerName = PlayerName.Replace("^3", "");
                            }
                            if (PlayerName.Contains("^4"))
                            {
                                PlayerName = PlayerName.Replace("^4", "");
                            }
                            if (PlayerName.Contains("^5"))
                            {
                                PlayerName = PlayerName.Replace("^5", "");
                            }
                            if (PlayerName.Contains("^6"))
                            {
                                PlayerName = PlayerName.Replace("^6", "");
                            }
                            if (PlayerName.Contains("^7"))
                            {
                                PlayerName = PlayerName.Replace("^7", "");
                            }
                            if (PlayerName.Contains("^8"))
                            {
                                PlayerName = PlayerName.Replace("^8", "");
                            }
                            #endregion

                            bool Complete = false;
                            // Your Command here!
                            if (FileInfo.GetUserAdmin(Username) == 1)
                            {
                                MsgPly("^9 " + PlayerName + " is already a Admin!", MSO.UCID);
                            }
                            else
                            {
                                if (IsModerator == 1)
                                {
                                    Complete = true;
                                    IsModerator = 0;
                                    MsgAll("^9 " + PlayerName + " is now removed as Moderator!");
                                    //AdmBox("> " + Conn.PlayerName + " removed " + PlayerName + " as Moderator!");
                                }
                                else if (IsModerator == 0)
                                {
                                    MsgPly("^9 " + PlayerName + " is not a Moderator!", MSO.UCID);
                                }
                            }
                            #region ' Save User '
                            if (Complete == true)
                            {
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);
                            }
                            #endregion
                        }
                        else
                        {
                            MsgPly("^9 " + Username + " wasn't found on database", MSO.UCID);
                        }
                    }

                    #endregion
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                    //AdmBox("> " + Conn.PlayerName + " (" + Conn.Username + ") tried to access the Remove Moderator Command!");
                    MsgAll("^9 " + Conn.PlayerName + " tried to use Remove Moderator Command!");
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("refund", "refund <amount> <username>")]
        public void refund(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "skywatcher122")
                {
                    try
                    {
                        bool Found = false;
                        string Username = Msg.Remove(0, 9 + StrMsg[1].Length);
                        int Refund = int.Parse(StrMsg[1]);

                        if (Refund.ToString().Contains("-"))
                        {
                            MsgPly("^9 Input Invalid. Don't use minus on the Values!", MSO.UCID);
                        }
                        else
                        {
                            #region ' Online Refunding '

                            foreach (clsConnection l in Connections)
                            {
                                if (l.Username == Username)
                                {
                                    Found = true;
                                    // All Players
                                    MsgAll("^9 " + l.PlayerName + " (" + l.Username + ") was refunded");
                                    MsgAll("^7Refunded by: " + Conn.PlayerName + " (" + Conn.Username + ")");
                                    MsgAll("^7Refund Amount: ^2$" + Refund);

                                    // To Admin Box

                                    l.Cash += Refund;
                                }
                            }

                            #endregion

                            #region ' Offline Refund '

                            if (Found == false)
                            {
                                if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                                {
                                    #region ' Objects '
                                    long Cash = FileInfo.GetUserCash(Username);
                                    long BBal = FileInfo.GetUserBank(Username);
                                    string Cars = FileInfo.GetUserCars(Username);
                                    long Gold = FileInfo.GetUserGold(Username);

                                    long TotalDistance = FileInfo.GetUserDistance(Username);
                                    byte TotalHealth = FileInfo.GetUserHealth(Username);
                                    int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                                    byte Electronics = FileInfo.GetUserElectronics(Username);
                                    byte Furniture = FileInfo.GetUserFurniture(Username);

                                    int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                                    int LastLotto = FileInfo.GetUserLastLotto(Username);

                                    byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                                    byte CanBeCadet = FileInfo.CanBeCadet(Username);
                                    byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                                    byte IsModerator = FileInfo.IsMember(Username);

                                    byte Interface1 = FileInfo.GetInterface(Username);
                                    byte Interface2 = FileInfo.GetInterface2(Username);
                                    byte Speedo = FileInfo.GetSpeedo(Username);
                                    byte Odometer = FileInfo.GetOdometer(Username);
                                    byte Counter = FileInfo.GetCounter(Username);
                                    byte Panel = FileInfo.GetCopPanel(Username);

                                    byte Renting = FileInfo.GetUserRenting(Username);
                                    byte Rented = FileInfo.GetUserRented(Username);
                                    string Renter = FileInfo.GetUserRenter(Username);
                                    string Renterr = FileInfo.GetUserRenterr(Username);
                                    string Rentee = FileInfo.GetUserRentee(Username);

                                    string PlayerName = FileInfo.GetUserPlayerName(Username);
                                    #endregion

                                    #region ' Special PlayerName Colors Remove '

                                    string NoColPlyName = PlayerName;
                                    if (PlayerName.Contains("^0"))
                                    {
                                        PlayerName = PlayerName.Replace("^0", "");
                                    }
                                    if (PlayerName.Contains("^1"))
                                    {
                                        PlayerName = PlayerName.Replace("^1", "");
                                    }
                                    if (PlayerName.Contains("^2"))
                                    {
                                        PlayerName = PlayerName.Replace("^2", "");
                                    }
                                    if (PlayerName.Contains("^3"))
                                    {
                                        PlayerName = PlayerName.Replace("^3", "");
                                    }
                                    if (PlayerName.Contains("^4"))
                                    {
                                        PlayerName = PlayerName.Replace("^4", "");
                                    }
                                    if (PlayerName.Contains("^5"))
                                    {
                                        PlayerName = PlayerName.Replace("^5", "");
                                    }
                                    if (PlayerName.Contains("^6"))
                                    {
                                        PlayerName = PlayerName.Replace("^6", "");
                                    }
                                    if (PlayerName.Contains("^7"))
                                    {
                                        PlayerName = PlayerName.Replace("^7", "");
                                    }
                                    if (PlayerName.Contains("^8"))
                                    {
                                        PlayerName = PlayerName.Replace("^8", "");
                                    }
                                    #endregion

                                    //bool Complete = false;
                                    // Your Command here!

                                    // All Players
                                    MsgAll("^9 " + PlayerName + " (" + Username + ") was refunded");
                                    MsgAll("^7Refunded by: " + Conn.PlayerName + " (" + Conn.Username + ")");
                                    MsgAll("^7Refund Amount: ^2$" + Refund);

                                    // To Admin Box

                                    Cash += Refund;

                                    #region ' Save User '
                                    FileInfo.SaveOfflineUser(Username,
                                        PlayerName,
                                        Cash,
                                        BBal,
                                        Cars,
                                        TotalHealth,
                                        TotalDistance,
                                        Gold,
                                        TotalJobsDone,
                                        Electronics,
                                        Furniture,
                                        IsModerator,
                                        CanBeOfficer,
                                        CanBeCadet,
                                        CanBeTowTruck,
                                        LastRaffle,
                                        LastLotto,
                                        Interface1,
                                        Interface2,
                                        Speedo,
                                        Odometer,
                                        Counter,
                                        Panel,
                                        Renting,
                                        Rented,
                                        Renter,
                                        Rentee,
                                        Renterr);

                                    #endregion
                                }
                                else
                                {
                                    MsgPly("^9 " + Username + " wasn't found on database", MSO.UCID);
                                }
                            }

                            #endregion
                        }
                    }
                    catch
                    {
                        MsgPly("^9 An Error has Occured. Re-consider the Amount!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                    //AdmBox("> " + Conn.PlayerName + " (" + Conn.Username + ") tried to access the Refund Command!");
                    MsgAll("^9 " + Conn.PlayerName + " tried to use Refund Command!");
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("fine", "fine <amount> <username>")]
        public void fine(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 2)
            {
                if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "skywatcher122" || Conn.IsModerator == 1)
                {
                    try
                    {
                        bool Found = false;
                        string Username = Msg.Remove(0, StrMsg[0].Length + StrMsg[1].Length + 2);
                        int Fine = int.Parse(StrMsg[1]);

                        if (Fine.ToString().Contains("-"))
                        {
                            MsgPly("^9 Input Invalid. Don't use minus on the Values!", MSO.UCID);
                        }
                        else
                        {
                            #region ' Online Force Fine '

                            foreach (clsConnection l in Connections)
                            {
                                if (l.Username == Username)
                                {
                                    Found = true;
                                    // All Players
                                    MsgAll("^9 " + l.PlayerName + " (" + l.Username + ") was Forced Fine");
                                    MsgAll("^7Fined by: " + Conn.PlayerName + " (" + Conn.Username + ")");
                                    MsgAll("^7Fine Amount: ^1$" + Fine);

                                    // To Admin Box

                                    l.Cash -= Fine;
                                }
                            }

                            #endregion

                            #region ' Offline Force Fine '

                            if (Found == false)
                            {
                                if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                                {
                                    #region ' Objects '
                                    long Cash = FileInfo.GetUserCash(Username);
                                    long BBal = FileInfo.GetUserBank(Username);
                                    string Cars = FileInfo.GetUserCars(Username);
                                    long Gold = FileInfo.GetUserGold(Username);

                                    long TotalDistance = FileInfo.GetUserDistance(Username);
                                    byte TotalHealth = FileInfo.GetUserHealth(Username);
                                    int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                                    byte Electronics = FileInfo.GetUserElectronics(Username);
                                    byte Furniture = FileInfo.GetUserFurniture(Username);

                                    int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                                    int LastLotto = FileInfo.GetUserLastLotto(Username);

                                    byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                                    byte CanBeCadet = FileInfo.CanBeCadet(Username);
                                    byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                                    byte IsModerator = FileInfo.IsMember(Username);

                                    byte Interface1 = FileInfo.GetInterface(Username);
                                    byte Interface2 = FileInfo.GetInterface2(Username);
                                    byte Speedo = FileInfo.GetSpeedo(Username);
                                    byte Odometer = FileInfo.GetOdometer(Username);
                                    byte Counter = FileInfo.GetCounter(Username);
                                    byte Panel = FileInfo.GetCopPanel(Username);

                                    byte Renting = FileInfo.GetUserRenting(Username);
                                    byte Rented = FileInfo.GetUserRented(Username);
                                    string Renter = FileInfo.GetUserRenter(Username);
                                    string Renterr = FileInfo.GetUserRenterr(Username);
                                    string Rentee = FileInfo.GetUserRentee(Username);

                                    string PlayerName = FileInfo.GetUserPlayerName(Username);
                                    #endregion

                                    #region ' Special PlayerName Colors Remove '

                                    string NoColPlyName = PlayerName;
                                    if (PlayerName.Contains("^0"))
                                    {
                                        PlayerName = PlayerName.Replace("^0", "");
                                    }
                                    if (PlayerName.Contains("^1"))
                                    {
                                        PlayerName = PlayerName.Replace("^1", "");
                                    }
                                    if (PlayerName.Contains("^2"))
                                    {
                                        PlayerName = PlayerName.Replace("^2", "");
                                    }
                                    if (PlayerName.Contains("^3"))
                                    {
                                        PlayerName = PlayerName.Replace("^3", "");
                                    }
                                    if (PlayerName.Contains("^4"))
                                    {
                                        PlayerName = PlayerName.Replace("^4", "");
                                    }
                                    if (PlayerName.Contains("^5"))
                                    {
                                        PlayerName = PlayerName.Replace("^5", "");
                                    }
                                    if (PlayerName.Contains("^6"))
                                    {
                                        PlayerName = PlayerName.Replace("^6", "");
                                    }
                                    if (PlayerName.Contains("^7"))
                                    {
                                        PlayerName = PlayerName.Replace("^7", "");
                                    }
                                    if (PlayerName.Contains("^8"))
                                    {
                                        PlayerName = PlayerName.Replace("^8", "");
                                    }
                                    #endregion

                                    //bool Complete = false;
                                    // Your Command here!

                                    // All Players
                                    MsgAll("^9 " + PlayerName + " (" + Username + ") was Forced Fine");
                                    MsgAll("^7Forced Fine by: " + Conn.PlayerName + " (" + Conn.Username + ")");
                                    MsgAll("^7Force Fine Amount: ^1$" + Fine);

                                    // To Admin Box

                                    Cash -= Fine;

                                    #region ' Save User '
                                    FileInfo.SaveOfflineUser(Username,
                                        PlayerName,
                                        Cash,
                                        BBal,
                                        Cars,
                                        TotalHealth,
                                        TotalDistance,
                                        Gold,
                                        TotalJobsDone,
                                        Electronics,
                                        Furniture,
                                        IsModerator,
                                        CanBeOfficer,
                                        CanBeCadet,
                                        CanBeTowTruck,
                                        LastRaffle,
                                        LastLotto,
                                        Interface1,
                                        Interface2,
                                        Speedo,
                                        Odometer,
                                        Counter,
                                        Panel,
                                        Renting,
                                        Rented,
                                        Renter,
                                        Rentee,
                                        Renterr);

                                    #endregion
                                }
                                else
                                {
                                    MsgPly("^9 " + Username + " wasn't found on database", MSO.UCID);
                                }
                            }

                            #endregion
                        }
                    }
                    catch
                    {
                        MsgPly("^9 An Error has Occured. Re-consider the Amount!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                    //AdmBox("> " + Conn.PlayerName + " (" + Conn.Username + ") tried to access the Fine Command!");
                    MsgAll("^9 " + Conn.PlayerName + " tried to use Fine Command!");
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("mod", "mod <username>")]
        public void mod(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsModerator == 1 || Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "skywatcher122")
                {
                    string Username = Msg.Remove(0, 5);
                    bool Found = false;

                    if (Conn.InFineMenu == false && Conn.AcceptTicket == 0)
                    {
                        if (Conn.InModerationMenu == 0)
                        {
                            #region ' Online '

                            foreach (clsConnection i in Connections)
                            {
                                if (i.Username == Username)
                                {
                                    Found = true;
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 77, 50, 50, 30, (Conn.UniqueID), 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 77, 50, 50, 31, (Conn.UniqueID), 2, false);
                                    InSim.Send_BTN_CreateButton("^7Moderation Window", Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 32, (Conn.UniqueID), 2, false);
                                    InSim.Send_BTN_CreateButton("^4> ^7Moderation Name: " + i.PlayerName + " ^7(" + i.Username + ")", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 5, 70, 60, 54, 33, (Conn.UniqueID), 2, false);
                                    InSim.Send_BTN_CreateButton("^4> ^7Cash: ^2$" + i.Cash + " ^7Bank Balance: ^2$" + i.BankBalance + " ^7Distance: ^2" + i.TotalDistance / 1000 + " km ^7/ ^2" + i.TotalDistance / 1609 + " mi", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 5, 70, 65, 54, 34, (Conn.UniqueID), 2, false);

                                    if (i.Cars.Length > 52 && i.Cars.Length < 84)
                                    {
                                        InSim.Send_BTN_CreateButton("^4> ^7Cars: " + i.Cars.Remove(39, i.Cars.Length - 39), Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 5, 70, 70, 54, 35, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^4> ^7" + i.Cars.Remove(0, 40), Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 5, 70, 75, 54, 36, Conn.UniqueID, 2, false);

                                    }
                                    else if (i.Cars.Length < 52)
                                    {
                                        InSim.Send_BTN_CreateButton("^4> ^7Cars: " + i.Cars, Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 5, 70, 70, 54, 35, Conn.UniqueID, 2, false);
                                    }
                                    InSim.Send_BTN_CreateButton("^4>> ^7Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 69, 82, 54, 52, 37, MSO.UCID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, MSO.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8FINE", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 68, 39, MSO.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, MSO.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, MSO.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8BAN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 110, 42, MSO.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1^J‚w", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 6, 52, 118, 43, MSO.UCID, 40, false);

                                    Conn.ModUsername = Username;
                                    Conn.InModerationMenu = 1;
                                }
                            }

                            #endregion

                            #region ' Offline '

                            if (Found == false)
                            {
                                if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                                {
                                    #region ' Objects '
                                    long Cash = FileInfo.GetUserCash(Username);
                                    long BBal = FileInfo.GetUserBank(Username);
                                    string Cars = FileInfo.GetUserCars(Username);
                                    long Gold = FileInfo.GetUserGold(Username);

                                    long TotalDistance = FileInfo.GetUserDistance(Username);
                                    byte TotalHealth = FileInfo.GetUserHealth(Username);
                                    int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                                    byte Electronics = FileInfo.GetUserElectronics(Username);
                                    byte Furniture = FileInfo.GetUserFurniture(Username);

                                    int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                                    int LastLotto = FileInfo.GetUserLastLotto(Username);

                                    byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                                    byte CanBeCadet = FileInfo.CanBeCadet(Username);
                                    byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                                    byte IsModerator = FileInfo.IsMember(Username);

                                    byte Interface1 = FileInfo.GetInterface(Username);
                                    byte Interface2 = FileInfo.GetInterface2(Username);
                                    byte Speedo = FileInfo.GetSpeedo(Username);
                                    byte Odometer = FileInfo.GetOdometer(Username);
                                    byte Counter = FileInfo.GetCounter(Username);
                                    byte Panel = FileInfo.GetCopPanel(Username);

                                    byte Renting = FileInfo.GetUserRenting(Username);
                                    byte Rented = FileInfo.GetUserRented(Username);
                                    string Renter = FileInfo.GetUserRenter(Username);
                                    string Renterr = FileInfo.GetUserRenterr(Username);
                                    string Rentee = FileInfo.GetUserRentee(Username);

                                    string PlayerName = FileInfo.GetUserPlayerName(Username);
                                    #endregion


                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 77, 50, 50, 30, (Conn.UniqueID), 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 77, 50, 50, 31, (Conn.UniqueID), 2, false);
                                    InSim.Send_BTN_CreateButton("^7Moderation Window", Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 32, (Conn.UniqueID), 2, false);
                                    InSim.Send_BTN_CreateButton("^4> ^7Moderation Name: " + PlayerName + " ^7(" + Username + ")", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 5, 70, 60, 54, 33, (Conn.UniqueID), 2, false);
                                    InSim.Send_BTN_CreateButton("^4> ^7Cash: ^2$" + Cash + " ^7Bank Balance: ^2$" + BBal + " ^7Distance: ^2" + TotalDistance / 1000 + " km ^7/ ^2" + TotalDistance / 1609 + " mi", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 5, 70, 65, 54, 34, (Conn.UniqueID), 2, false);

                                    if (Cars.Length > 52 && Cars.Length < 84)
                                    {
                                        InSim.Send_BTN_CreateButton("^4> ^7Cars: " + Cars.Remove(39, Cars.Length - 39), Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 5, 70, 70, 54, 35, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^4> ^7" + Cars.Remove(0, 40), Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 5, 70, 75, 54, 36, Conn.UniqueID, 2, false);

                                    }
                                    else if (Cars.Length < 52)
                                    {
                                        InSim.Send_BTN_CreateButton("^4> ^7Cars: " + Cars, Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 5, 70, 70, 54, 35, Conn.UniqueID, 2, false);
                                    }
                                    InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, Conn.UniqueID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                    InSim.Send_BTN_CreateButton("^1^J‚w", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 6, 52, 118, 43, MSO.UCID, 40, false);


                                    Conn.ModUsername = Username;
                                    Conn.InModerationMenu = 2;
                                }
                                else
                                {
                                    MsgPly("^9 " + Username + " wasn't found on database", MSO.UCID);
                                }

                            }

                            #endregion
                        }
                        else
                        {
                            MsgPly("^9 Close your current Moderation first!", MSO.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^9 Complete the Ticket Menu Panel First!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                    //AdmBox("> " + Conn.PlayerName + " (" + Conn.Username + ") tried to access the Moderation Command!");
                    MsgAll("^9 " + Conn.PlayerName + " tried to use Moderation Command!");
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("banlist", "banlist <username>")]
        public void banlist(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "skywatcher122")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 9);
                    #region ' Online '

                    foreach (clsConnection i in Connections)
                    {
                        if (i.Username == Username)
                        {
                            Found = true;
                            MsgAll("^9 " + i.PlayerName + " was banned by " + Conn.PlayerName);
                            MsgPly("^9 You are banlisted by: " + Conn.PlayerName, i.UniqueID);
                            //AdmBox("> " + Conn.PlayerName + " added " + i.PlayerName + " (" + i.Username + ") to the user banlist!");
                            FileInfo.AddBanList(Username);
                            BanID(i.Username, 0);
                        }
                    }

                    #endregion

                    #region ' Offline '

                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (PlayerName.Contains("^0"))
                            {
                                PlayerName = PlayerName.Replace("^0", "");
                            }
                            if (PlayerName.Contains("^1"))
                            {
                                PlayerName = PlayerName.Replace("^1", "");
                            }
                            if (PlayerName.Contains("^2"))
                            {
                                PlayerName = PlayerName.Replace("^2", "");
                            }
                            if (PlayerName.Contains("^3"))
                            {
                                PlayerName = PlayerName.Replace("^3", "");
                            }
                            if (PlayerName.Contains("^4"))
                            {
                                PlayerName = PlayerName.Replace("^4", "");
                            }
                            if (PlayerName.Contains("^5"))
                            {
                                PlayerName = PlayerName.Replace("^5", "");
                            }
                            if (PlayerName.Contains("^6"))
                            {
                                PlayerName = PlayerName.Replace("^6", "");
                            }
                            if (PlayerName.Contains("^7"))
                            {
                                PlayerName = PlayerName.Replace("^7", "");
                            }
                            if (PlayerName.Contains("^8"))
                            {
                                PlayerName = PlayerName.Replace("^8", "");
                            }
                            #endregion

                            MsgAll("^9 " + PlayerName + " was banned by " + Conn.PlayerName);
                            //AdmBox("> " + Conn.PlayerName + " added " + PlayerName + " (" + Username + ") to the user banlist!");
                            FileInfo.AddBanList(Username);
                            BanID(Username, 0);
                        }
                        else
                        {
                            MsgPly("^9 " + Username + " wasn't found in database!", MSO.UCID);
                        }
                    }

                    #endregion
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                    // AdmBox("> " + Conn.PlayerName + " (" + Conn.Username + ") tried to access the Banlist Command!");
                    MsgAll("^9 " + Conn.PlayerName + " tried to use Banlist Command!");
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("refdist", "refdist <distance> <username>")]
        public void refdist(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 2)
            {
                if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "skywatcher122")
                {
                    string Username = Msg.Remove(0, StrMsg[0].Length + StrMsg[1].Length + 2);
                    bool Found = false;
                    int Amount = int.Parse(StrMsg[1]);

                    try
                    {
                        if (Amount.ToString().Contains("-"))
                        {
                            MsgPly("^9 Input Invalid. Don't use minus on the Values!", MSO.UCID);
                        }
                        else
                        {
                            #region ' Online '

                            foreach (clsConnection i in Connections)
                            {
                                if (i.Username == Username)
                                {
                                    Found = true;
                                    MsgAll("^9 " + i.PlayerName + " (" + i.Username + ") was refunded in Distance");
                                    MsgAll("^7Refunded by: " + Conn.PlayerName + " (" + Conn.Username + ")");
                                    MsgAll("^7Refund Distance: ^2" + Amount + " km");
                                    i.TotalDistance += Amount * 1000;

                                }
                            }

                            #endregion

                            #region ' Offline '

                            if (Found == false)
                            {
                                if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                                {
                                    #region ' Objects '
                                    long Cash = FileInfo.GetUserCash(Username);
                                    long BBal = FileInfo.GetUserBank(Username);
                                    string Cars = FileInfo.GetUserCars(Username);
                                    long Gold = FileInfo.GetUserGold(Username);

                                    long TotalDistance = FileInfo.GetUserDistance(Username);
                                    byte TotalHealth = FileInfo.GetUserHealth(Username);
                                    int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                                    byte Electronics = FileInfo.GetUserElectronics(Username);
                                    byte Furniture = FileInfo.GetUserFurniture(Username);

                                    int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                                    int LastLotto = FileInfo.GetUserLastLotto(Username);

                                    byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                                    byte CanBeCadet = FileInfo.CanBeCadet(Username);
                                    byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                                    byte IsModerator = FileInfo.IsMember(Username);

                                    byte Interface1 = FileInfo.GetInterface(Username);
                                    byte Interface2 = FileInfo.GetInterface2(Username);
                                    byte Speedo = FileInfo.GetSpeedo(Username);
                                    byte Odometer = FileInfo.GetOdometer(Username);
                                    byte Counter = FileInfo.GetCounter(Username);
                                    byte Panel = FileInfo.GetCopPanel(Username);

                                    byte Renting = FileInfo.GetUserRenting(Username);
                                    byte Rented = FileInfo.GetUserRented(Username);
                                    string Renter = FileInfo.GetUserRenter(Username);
                                    string Renterr = FileInfo.GetUserRenterr(Username);
                                    string Rentee = FileInfo.GetUserRentee(Username);

                                    string PlayerName = FileInfo.GetUserPlayerName(Username);
                                    #endregion

                                    #region ' Special PlayerName Colors Remove '

                                    string NoColPlyName = PlayerName;
                                    if (PlayerName.Contains("^0"))
                                    {
                                        PlayerName = PlayerName.Replace("^0", "");
                                    }
                                    if (PlayerName.Contains("^1"))
                                    {
                                        PlayerName = PlayerName.Replace("^1", "");
                                    }
                                    if (PlayerName.Contains("^2"))
                                    {
                                        PlayerName = PlayerName.Replace("^2", "");
                                    }
                                    if (PlayerName.Contains("^3"))
                                    {
                                        PlayerName = PlayerName.Replace("^3", "");
                                    }
                                    if (PlayerName.Contains("^4"))
                                    {
                                        PlayerName = PlayerName.Replace("^4", "");
                                    }
                                    if (PlayerName.Contains("^5"))
                                    {
                                        PlayerName = PlayerName.Replace("^5", "");
                                    }
                                    if (PlayerName.Contains("^6"))
                                    {
                                        PlayerName = PlayerName.Replace("^6", "");
                                    }
                                    if (PlayerName.Contains("^7"))
                                    {
                                        PlayerName = PlayerName.Replace("^7", "");
                                    }
                                    if (PlayerName.Contains("^8"))
                                    {
                                        PlayerName = PlayerName.Replace("^8", "");
                                    }
                                    #endregion

                                    //bool Complete = false;
                                    // Your Command here!

                                    // All Players
                                    MsgAll("^9 " + PlayerName + " (" + Username + ") was refunded in Distance");
                                    MsgAll("^7Refunded by: " + Conn.PlayerName + " (" + Conn.Username + ")");
                                    MsgAll("^7Refund Distance: ^2" + Amount + " km");

                                    // To Admin Box

                                    TotalDistance += Amount * 1000;

                                    #region ' Save User '
                                    FileInfo.SaveOfflineUser(Username,
                                        PlayerName,
                                        Cash,
                                        BBal,
                                        Cars,
                                        TotalHealth,
                                        TotalDistance,
                                        Gold,
                                        TotalJobsDone,
                                        Electronics,
                                        Furniture,
                                        IsModerator,
                                        CanBeOfficer,
                                        CanBeCadet,
                                        CanBeTowTruck,
                                        LastRaffle,
                                        LastLotto,
                                        Interface1,
                                        Interface2,
                                        Speedo,
                                        Odometer,
                                        Counter,
                                        Panel,
                                        Renting,
                                        Rented,
                                        Renter,
                                        Rentee,
                                        Renterr);

                                    #endregion
                                }
                                else
                                {
                                    MsgPly("^9 " + Username + " wasn't found on database", MSO.UCID);
                                }
                            }

                            #endregion
                        }
                    }
                    catch
                    {
                        MsgPly("^9 An Error has Occured. Re-consider the Amount!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                    //AdmBox("> " + Conn.PlayerName + " (" + Conn.Username + ") tried to access the Refund distance Command!");
                    MsgAll("^9 " + Conn.PlayerName + " tried to use Refund distance Command!");
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("finedist", "finedist <distance> <username>")]
        public void finedist(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 2)
            {
                if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "skywatcher122")
                {
                    string Username = Msg.Remove(0, StrMsg[0].Length + StrMsg[1].Length + 2);
                    bool Found = false;
                    int Amount = int.Parse(StrMsg[1]);

                    try
                    {
                        if (Amount.ToString().Contains("-"))
                        {
                            MsgPly("^9 Input Invalid. Don't use minus on the Values!", MSO.UCID);
                        }
                        else
                        {
                            #region ' Online '

                            foreach (clsConnection i in Connections)
                            {
                                if (i.Username == Username)
                                {
                                    Found = true;
                                    MsgAll("^9 " + i.PlayerName + " (" + i.Username + ") was fined in Distance");
                                    MsgAll("^7Fined by: " + Conn.PlayerName + " (" + Conn.Username + ")");
                                    MsgAll("^7Fined Distance: ^1" + Amount + " km");
                                    i.TotalDistance -= Amount * 1000;


                                }
                            }

                            #endregion

                            #region ' Offline '

                            if (Found == false)
                            {
                                if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                                {
                                    #region ' Objects '
                                    long Cash = FileInfo.GetUserCash(Username);
                                    long BBal = FileInfo.GetUserBank(Username);
                                    string Cars = FileInfo.GetUserCars(Username);
                                    long Gold = FileInfo.GetUserGold(Username);

                                    long TotalDistance = FileInfo.GetUserDistance(Username);
                                    byte TotalHealth = FileInfo.GetUserHealth(Username);
                                    int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                                    byte Electronics = FileInfo.GetUserElectronics(Username);
                                    byte Furniture = FileInfo.GetUserFurniture(Username);

                                    int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                                    int LastLotto = FileInfo.GetUserLastLotto(Username);

                                    byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                                    byte CanBeCadet = FileInfo.CanBeCadet(Username);
                                    byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                                    byte IsModerator = FileInfo.IsMember(Username);

                                    byte Interface1 = FileInfo.GetInterface(Username);
                                    byte Interface2 = FileInfo.GetInterface2(Username);
                                    byte Speedo = FileInfo.GetSpeedo(Username);
                                    byte Odometer = FileInfo.GetOdometer(Username);
                                    byte Counter = FileInfo.GetCounter(Username);
                                    byte Panel = FileInfo.GetCopPanel(Username);

                                    byte Renting = FileInfo.GetUserRenting(Username);
                                    byte Rented = FileInfo.GetUserRented(Username);
                                    string Renter = FileInfo.GetUserRenter(Username);
                                    string Renterr = FileInfo.GetUserRenterr(Username);
                                    string Rentee = FileInfo.GetUserRentee(Username);

                                    string PlayerName = FileInfo.GetUserPlayerName(Username);
                                    #endregion

                                    #region ' Special PlayerName Colors Remove '

                                    string NoColPlyName = PlayerName;
                                    if (PlayerName.Contains("^0"))
                                    {
                                        PlayerName = PlayerName.Replace("^0", "");
                                    }
                                    if (PlayerName.Contains("^1"))
                                    {
                                        PlayerName = PlayerName.Replace("^1", "");
                                    }
                                    if (PlayerName.Contains("^2"))
                                    {
                                        PlayerName = PlayerName.Replace("^2", "");
                                    }
                                    if (PlayerName.Contains("^3"))
                                    {
                                        PlayerName = PlayerName.Replace("^3", "");
                                    }
                                    if (PlayerName.Contains("^4"))
                                    {
                                        PlayerName = PlayerName.Replace("^4", "");
                                    }
                                    if (PlayerName.Contains("^5"))
                                    {
                                        PlayerName = PlayerName.Replace("^5", "");
                                    }
                                    if (PlayerName.Contains("^6"))
                                    {
                                        PlayerName = PlayerName.Replace("^6", "");
                                    }
                                    if (PlayerName.Contains("^7"))
                                    {
                                        PlayerName = PlayerName.Replace("^7", "");
                                    }
                                    if (PlayerName.Contains("^8"))
                                    {
                                        PlayerName = PlayerName.Replace("^8", "");
                                    }
                                    #endregion

                                    //bool Complete = false;
                                    // Your Command here!

                                    // All Players
                                    MsgAll("^9 " + PlayerName + " (" + Username + ") was fined in Distance");
                                    MsgAll("^7Fined by: " + Conn.PlayerName + " (" + Conn.Username + ")");
                                    MsgAll("^7Fined Distance: ^1" + Amount + " km");

                                    // To Admin Box

                                    TotalDistance -= Amount * 1000;

                                    #region ' Save User '
                                    FileInfo.SaveOfflineUser(Username,
                                        PlayerName,
                                        Cash,
                                        BBal,
                                        Cars,
                                        TotalHealth,
                                        TotalDistance,
                                        Gold,
                                        TotalJobsDone,
                                        Electronics,
                                        Furniture,
                                        IsModerator,
                                        CanBeOfficer,
                                        CanBeCadet,
                                        CanBeTowTruck,
                                        LastRaffle,
                                        LastLotto,
                                        Interface1,
                                        Interface2,
                                        Speedo,
                                        Odometer,
                                        Counter,
                                        Panel,
                                        Renting,
                                        Rented,
                                        Renter,
                                        Rentee,
                                        Renterr);

                                    #endregion
                                }
                                else
                                {
                                    MsgPly("^9 " + Username + " wasn't found on database", MSO.UCID);
                                }
                            }

                            #endregion
                        }
                    }
                    catch
                    {
                        MsgPly("^9 An Error has Occured. Re-consider the Amount!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                    //AdmBox("> " + Conn.PlayerName + " (" + Conn.Username + ") tried to access the fine distance Command!");
                    MsgAll("^9 " + Conn.PlayerName + " tried to use fine distance Command!");
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("refcar", "refcar <car> <username>")]
        public void refcar(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 2)
            {
                if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "skywatcher122")
                {
                    bool Found = false;
                    string Username = Msg.Substring(StrMsg[0].Length + StrMsg[1].Length + 2);
                    string RefundCar = StrMsg[1].ToUpper();
                    try
                    {
                        #region ' Online '
                        foreach (clsConnection i in Connections)
                        {
                            if (i.Username == Username)
                            {
                                Found = true;
                                #region ' Exist '
                                if (i.Cars.Contains(RefundCar))
                                {
                                    if (RefundCar == "UF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| UF1000 (UF1) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XFG")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| XF GTi (XFG) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XRG")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| XR GTi (XRG) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "LX4")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| LX4 (LX4) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "LX6")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| LX6 (LX6) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "RB4")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| RB GT Turbo (RB4) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FXO")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| FX GT Turbo (FXO) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "VWS")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| Volkswagen Scirroco (VWS) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XRT")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| XR GT Turbo (XRT) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "RAC")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| Raceabout (RAC) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FZ5")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| FZ50 (FZ5) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "UFR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| UF GTR (UFR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XFR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| XF GTR (XFR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FXR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| FX GTR (FXR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XRR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| XR GTR (XRR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FZR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| FZ GTR (FZR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "MRT")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| McGill Racing Kart (MRT) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FOX")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| Formula XR (FOX) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FBM")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| Formula BMW FB02 (MRT) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FO8")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| Formula V8 (FO8) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "BF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| BMW Sauber 1.06 (BF1) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                }
                                #endregion

                                #region ' Coudn't be added '

                                else if (Dealer.GetCarPrice(RefundCar) == 0)
                                {
                                    if (RefundCar == "UF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| UF1000 (UF1) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XFG")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| XF GTi (XFG) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FO8")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| Formula V8 (FO8) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FOX")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| Formula XR (FOX) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "BF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| BMW Sauber F1.06 (BF1) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| " + RefundCar + " ^7does not exist on Dealership", MSO.UCID, 0);
                                    }
                                }

                                #endregion

                                #region ' Add '
                                else
                                {
                                    switch (RefundCar)
                                    {
                                        case "UF1":
                                            i.Cars += " " + "UF1";
                                            break;
                                        case "XFG":
                                            i.Cars += " " + "XFG";
                                            break;
                                        case "XRG":
                                            i.Cars += " " + "XRG";
                                            break;
                                        case "VWS":
                                            i.Cars += " " + "VWS";
                                            break;
                                        case "LX4":
                                            i.Cars += " " + "LX4";
                                            break;
                                        case "LX6":
                                            i.Cars += " " + "LX6";
                                            break;
                                        case "RB4":
                                            i.Cars += " " + "RB4";
                                            break;
                                        case "FXO":
                                            i.Cars += " " + "FXO";
                                            break;
                                        case "XRT":
                                            i.Cars += " " + "XRT";
                                            break;
                                        case "RAC":
                                            i.Cars += " " + "RAC";
                                            break;
                                        case "FZ5":
                                            i.Cars += " " + "FZ5";
                                            break;
                                        case "UFR":
                                            i.Cars += " " + "UFR";
                                            break;
                                        case "XFR":
                                            i.Cars += " " + "XFR";
                                            break;
                                        case "FXR":
                                            i.Cars += " " + "FXR";
                                            break;
                                        case "XRR":
                                            i.Cars += " " + "XRR";
                                            break;
                                        case "FZR":
                                            i.Cars += " " + "FZR";
                                            break;

                                        case "MRT":
                                            i.Cars += " " + "MRT";
                                            break;

                                        case "FBM":
                                            i.Cars += " " + "FBM";
                                            break;
                                    }

                                    MsgAll("^9 " + i.PlayerName + " received a " + RefundCar + " from " + Conn.PlayerName);

                                    //AdmBox("> " + Conn.PlayerName + " refunded " + i.PlayerName + " a " + RefundCar);

                                }
                                #endregion
                            }
                        }
                        #endregion

                        #region ' Offline '

                        if (Found == false)
                        {
                            if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                            {
                                #region ' Objects '
                                long Cash = FileInfo.GetUserCash(Username);
                                long BBal = FileInfo.GetUserBank(Username);
                                string Cars = FileInfo.GetUserCars(Username);
                                long Gold = FileInfo.GetUserGold(Username);

                                long TotalDistance = FileInfo.GetUserDistance(Username);
                                byte TotalHealth = FileInfo.GetUserHealth(Username);
                                int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                                byte Electronics = FileInfo.GetUserElectronics(Username);
                                byte Furniture = FileInfo.GetUserFurniture(Username);

                                int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                                int LastLotto = FileInfo.GetUserLastLotto(Username);

                                byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                                byte CanBeCadet = FileInfo.CanBeCadet(Username);
                                byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                                byte IsModerator = FileInfo.IsMember(Username);

                                byte Interface1 = FileInfo.GetInterface(Username);
                                byte Interface2 = FileInfo.GetInterface2(Username);
                                byte Speedo = FileInfo.GetSpeedo(Username);
                                byte Odometer = FileInfo.GetOdometer(Username);
                                byte Counter = FileInfo.GetCounter(Username);
                                byte Panel = FileInfo.GetCopPanel(Username);

                                byte Renting = FileInfo.GetUserRenting(Username);
                                byte Rented = FileInfo.GetUserRented(Username);
                                string Renter = FileInfo.GetUserRenter(Username);
                                string Renterr = FileInfo.GetUserRenterr(Username);
                                string Rentee = FileInfo.GetUserRentee(Username);

                                string PlayerName = FileInfo.GetUserPlayerName(Username);
                                #endregion

                                #region ' Special PlayerName Colors Remove '

                                string NoColPlyName = PlayerName;
                                if (PlayerName.Contains("^0"))
                                {
                                    PlayerName = PlayerName.Replace("^0", "");
                                }
                                if (PlayerName.Contains("^1"))
                                {
                                    PlayerName = PlayerName.Replace("^1", "");
                                }
                                if (PlayerName.Contains("^2"))
                                {
                                    PlayerName = PlayerName.Replace("^2", "");
                                }
                                if (PlayerName.Contains("^3"))
                                {
                                    PlayerName = PlayerName.Replace("^3", "");
                                }
                                if (PlayerName.Contains("^4"))
                                {
                                    PlayerName = PlayerName.Replace("^4", "");
                                }
                                if (PlayerName.Contains("^5"))
                                {
                                    PlayerName = PlayerName.Replace("^5", "");
                                }
                                if (PlayerName.Contains("^6"))
                                {
                                    PlayerName = PlayerName.Replace("^6", "");
                                }
                                if (PlayerName.Contains("^7"))
                                {
                                    PlayerName = PlayerName.Replace("^7", "");
                                }
                                if (PlayerName.Contains("^8"))
                                {
                                    PlayerName = PlayerName.Replace("^8", "");
                                }
                                #endregion

                                #region ' Exist '
                                if (Cars.Contains(RefundCar))
                                {
                                    if (RefundCar == "UF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| UF1000 (UF1) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XFG")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| XF GTi (XFG) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XRG")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| XR GTi (XRG) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "LX4")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| LX4 (LX4) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "LX6")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| LX6 (LX6) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "RB4")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| RB GT Turbo (RB4) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FXO")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| FX GT Turbo (FXO) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "VWS")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| Volkswagen Scirroco (VWS) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XRT")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| XR GT Turbo (XRT) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "RAC")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| Raceabout (RAC) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FZ5")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| FZ50 (FZ5) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "UFR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| UF GTR (UFR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XFR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| XF GTR (XFR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FXR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| FX GTR (FXR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XRR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| XR GTR (XRR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FZR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| FZ GTR (FZR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "MRT")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| McGill Racing Kart (MRT) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FOX")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| Formula XR (FOX) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FBM")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| Formula BMW FB02 (MRT) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FO8")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| Formula V8 (FO8) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "BF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| BMW Sauber 1.06 (BF1) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                }
                                #endregion

                                #region ' Coudn't be added '

                                else if (Dealer.GetCarPrice(RefundCar) == 0)
                                {
                                    if (RefundCar == "UF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| UF1000 (UF1) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XFG")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| XF GTi (XFG) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FO8")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| Formula V8 (FO8) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FOX")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| Formula XR (FOX) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "BF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| BMW Sauber F1.06 (BF1) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else
                                    {
                                        InSim.Send_MTC_MessageToConnection("^4| " + RefundCar + " ^7does not exist on Dealership", MSO.UCID, 0);
                                    }
                                }

                                #endregion

                                #region ' Add '
                                else
                                {
                                    switch (RefundCar)
                                    {
                                        case "UF1":
                                            Cars += " " + "UF1";
                                            break;
                                        case "XFG":
                                            Cars += " " + "XFG";
                                            break;
                                        case "XRG":
                                            Cars += " " + "XRG";
                                            break;
                                        case "VWS":
                                            Cars += " " + "VWS";
                                            break;
                                        case "LX4":
                                            Cars += " " + "LX4";
                                            break;
                                        case "LX6":
                                            Cars += " " + "LX6";
                                            break;
                                        case "RB4":
                                            Cars += " " + "RB4";
                                            break;
                                        case "FXO":
                                            Cars += " " + "FXO";
                                            break;
                                        case "XRT":
                                            Cars += " " + "XRT";
                                            break;
                                        case "RAC":
                                            Cars += " " + "RAC";
                                            break;
                                        case "FZ5":
                                            Cars += " " + "FZ5";
                                            break;
                                        case "UFR":
                                            Cars += " " + "UFR";
                                            break;
                                        case "XFR":
                                            Cars += " " + "XFR";
                                            break;
                                        case "FXR":
                                            Cars += " " + "FXR";
                                            break;
                                        case "XRR":
                                            Cars += " " + "XRR";
                                            break;
                                        case "FZR":
                                            Cars += " " + "FZR";
                                            break;

                                        case "MRT":
                                            Cars += " " + "MRT";
                                            break;

                                        case "FBM":
                                            Cars += " " + "FBM";
                                            break;
                                    }

                                    MsgAll("^9 " + PlayerName + " received a " + RefundCar + " from " + Conn.PlayerName);


                                }
                                #endregion

                                #region ' Save User '
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);

                                #endregion
                            }
                            else
                            {
                                MsgPly("^9 " + Username + " wasn't found on database", MSO.UCID);
                            }
                        }

                        #endregion
                    }
                    catch
                    {
                        MsgPly("^9 An Error has occured. Please retype the Command!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                    //AdmBox("> " + Conn.PlayerName + " (" + Conn.Username + ") tried to access the refund vehicle Command!");
                    MsgAll("^9 " + Conn.PlayerName + " tried to use refund vehicle Command!");
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("remcar", "remcar <car> <username>")]
        public void remcar(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 2)
            {
                if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "skywatcher122")
                {
                    bool Found = false;
                    string Username = Msg.Substring(StrMsg[0].Length + StrMsg[1].Length + 2);
                    string RemoveCar = StrMsg[1].ToUpper();
                    try
                    {
                        #region ' Online '

                        foreach (clsConnection i in Connections)
                        {
                            if (i.Username == Username)
                            {
                                Found = true;
                                #region ' Check Owned '
                                if (i.Cars.Contains(RemoveCar))
                                {
                                    #region ' Check canbe Removed '
                                    if (Dealer.GetCarPrice(RemoveCar) > 0)
                                    {
                                        string UserCars = i.Cars;
                                        int IdxCar = UserCars.IndexOf(RemoveCar);
                                        try
                                        {
                                            i.Cars = i.Cars.Remove(IdxCar, 4);
                                        }
                                        catch
                                        {
                                            i.Cars = i.Cars.Remove(IdxCar, 3);
                                        }
                                        MsgAll("^9 " + i.PlayerName + " was force removed ^3" + RemoveCar + "^7!");

                                        //AdmBox("> " + Conn.PlayerName + " removed " + i.PlayerName + "'s " + RemoveCar + "!");
                                    }
                                    #endregion

                                    #region ' Check if couldn't removed '
                                    else
                                    {
                                        if (RemoveCar == "UF1")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^4| UF1000 (UF1) ^7coudn't be removed", MSO.UCID, 0);
                                        }
                                        else if (RemoveCar == "XFG")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^4| XF GTi (XFG) ^7coudn't be removed", MSO.UCID, 0);
                                        }
                                        else if (RemoveCar == "FO8")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^4| Formula V8 (FO8) ^7coudn't be removed", MSO.UCID, 0);
                                        }
                                        else if (RemoveCar == "FOX")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^4| Formula XR (FOX) ^7coudn't be removed", MSO.UCID, 0);
                                        }
                                        else if (RemoveCar == "BF1")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^4| BMW Sauber F1.06 (BF1) ^7coudn't be removed", MSO.UCID, 0);
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^4| " + RemoveCar + " ^7Invalid Car Garage List", MSO.UCID, 0);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                #region ' Check Doesn't Own '
                                else
                                {
                                    if (RemoveCar == "UF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1UF1000 (UF1)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "XFG")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1XF GTi (XFG)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "XRG")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1XR GTi (XRG)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "LX4")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1LX4 (LX4)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "LX6")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1LX6 (LX6)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "RB4")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1RB GT Turbo (RB4)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FXO")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1FX GT Turbo (FXO)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "VWS")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1Volkswagen Scirroco (VWS)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "XRT")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1XR GT Turbo (XRT)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "RAC")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1Raceabout (RAC)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FZ5")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1FZ50 GT (FZ5)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "UFR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1UF GTR (UFR)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "XFR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1XF GTR (XFR)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FXR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1FX GTR (FXR)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "XRR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1XR GTR (XRR)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FZR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1FZ GTR (FZR)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "MRT")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1McGill Racing Kart (MRT)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FBM")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1Formula BMW FB02 (FBM)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FO8")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1Formula V8 (FO8)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FOX")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1Formula XR (FOX)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "BF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + i.PlayerName + " doesn't own ^1BMW Sauber F1.06 (BF1)", MSO.UCID, 0);
                                    }
                                    else
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + RemoveCar + " ^7Invalid Car Garage List", MSO.UCID, 0);
                                    }
                                }
                                #endregion
                            }
                        }

                        #endregion

                        #region ' Offline '

                        if (Found == false)
                        {
                            if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                            {
                                #region ' Objects '
                                long Cash = FileInfo.GetUserCash(Username);
                                long BBal = FileInfo.GetUserBank(Username);
                                string Cars = FileInfo.GetUserCars(Username);
                                long Gold = FileInfo.GetUserGold(Username);

                                long TotalDistance = FileInfo.GetUserDistance(Username);
                                byte TotalHealth = FileInfo.GetUserHealth(Username);
                                int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                                byte Electronics = FileInfo.GetUserElectronics(Username);
                                byte Furniture = FileInfo.GetUserFurniture(Username);

                                int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                                int LastLotto = FileInfo.GetUserLastLotto(Username);

                                byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                                byte CanBeCadet = FileInfo.CanBeCadet(Username);
                                byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                                byte IsModerator = FileInfo.IsMember(Username);

                                byte Interface1 = FileInfo.GetInterface(Username);
                                byte Interface2 = FileInfo.GetInterface2(Username);
                                byte Speedo = FileInfo.GetSpeedo(Username);
                                byte Odometer = FileInfo.GetOdometer(Username);
                                byte Counter = FileInfo.GetCounter(Username);
                                byte Panel = FileInfo.GetCopPanel(Username);

                                byte Renting = FileInfo.GetUserRenting(Username);
                                byte Rented = FileInfo.GetUserRented(Username);
                                string Renter = FileInfo.GetUserRenter(Username);
                                string Renterr = FileInfo.GetUserRenterr(Username);
                                string Rentee = FileInfo.GetUserRentee(Username);

                                string PlayerName = FileInfo.GetUserPlayerName(Username);
                                #endregion

                                #region ' Special PlayerName Colors Remove '

                                string NoColPlyName = PlayerName;
                                if (PlayerName.Contains("^0"))
                                {
                                    PlayerName = PlayerName.Replace("^0", "");
                                }
                                if (PlayerName.Contains("^1"))
                                {
                                    PlayerName = PlayerName.Replace("^1", "");
                                }
                                if (PlayerName.Contains("^2"))
                                {
                                    PlayerName = PlayerName.Replace("^2", "");
                                }
                                if (PlayerName.Contains("^3"))
                                {
                                    PlayerName = PlayerName.Replace("^3", "");
                                }
                                if (PlayerName.Contains("^4"))
                                {
                                    PlayerName = PlayerName.Replace("^4", "");
                                }
                                if (PlayerName.Contains("^5"))
                                {
                                    PlayerName = PlayerName.Replace("^5", "");
                                }
                                if (PlayerName.Contains("^6"))
                                {
                                    PlayerName = PlayerName.Replace("^6", "");
                                }
                                if (PlayerName.Contains("^7"))
                                {
                                    PlayerName = PlayerName.Replace("^7", "");
                                }
                                if (PlayerName.Contains("^8"))
                                {
                                    PlayerName = PlayerName.Replace("^8", "");
                                }
                                #endregion

                                #region ' Check Owned '
                                if (Cars.Contains(RemoveCar))
                                {
                                    #region ' Check canbe Removed '
                                    if (Dealer.GetCarPrice(RemoveCar) > 0)
                                    {
                                        string UserCars = Cars;
                                        int IdxCar = UserCars.IndexOf(RemoveCar);
                                        try
                                        {
                                            Cars = Cars.Remove(IdxCar, 4);
                                        }
                                        catch
                                        {
                                            Cars = Cars.Remove(IdxCar, 3);
                                        }
                                        MsgAll("^9 " + PlayerName + " was force removed ^3" + RemoveCar + "^7!");
                                        //AdmBox("> " + Conn.PlayerName + " removed " + PlayerName + "'s " + RemoveCar + "!");
                                    }
                                    #endregion

                                    #region ' Check if couldn't removed '
                                    else
                                    {
                                        if (RemoveCar == "UF1")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^4| UF1000 (UF1) ^7coudn't be removed", MSO.UCID, 0);
                                        }
                                        else if (RemoveCar == "XFG")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^4| XF GTi (XFG) ^7coudn't be removed", MSO.UCID, 0);
                                        }
                                        else if (RemoveCar == "FO8")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^4| Formula V8 (FO8) ^7coudn't be removed", MSO.UCID, 0);
                                        }
                                        else if (RemoveCar == "FOX")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^4| Formula XR (FOX) ^7coudn't be removed", MSO.UCID, 0);
                                        }
                                        else if (RemoveCar == "BF1")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^4| BMW Sauber F1.06 (BF1) ^7coudn't be removed", MSO.UCID, 0);
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^4| " + RemoveCar + " ^7Invalid Car Garage List", MSO.UCID, 0);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                #region ' Check Doesn't Own '
                                else
                                {
                                    if (RemoveCar == "UF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1UF1000 (UF1)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "XFG")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1XF GTi (XFG)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "XRG")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1XR GTi (XRG)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "LX4")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1LX4 (LX4)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "LX6")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1LX6 (LX6)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "RB4")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1RB GT Turbo (RB4)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FXO")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1FX GT Turbo (FXO)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "VWS")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1Volkswagen Scirroco (VWS)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "XRT")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1XR GT Turbo (XRT)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "RAC")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1Raceabout (RAC)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FZ5")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1FZ50 GT (FZ5)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "UFR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1UF GTR (UFR)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "XFR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1XF GTR (XFR)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FXR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1FX GTR (FXR)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "XRR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1XR GTR (XRR)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FZR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1FZ GTR (FZR)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "MRT")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1McGill Racing Kart (MRT)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FBM")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1Formula BMW FB02 (FBM)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FO8")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1Formula V8 (FO8)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FOX")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1Formula XR (FOX)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "BF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + PlayerName + " doesn't own ^1BMW Sauber F1.06 (BF1)", MSO.UCID, 0);
                                    }
                                    else
                                    {
                                        InSim.Send_MTC_MessageToConnection("^9 " + RemoveCar + " ^7Invalid Car Garage List", MSO.UCID, 0);
                                    }
                                }
                                #endregion

                                #region ' Save User '
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);

                                #endregion
                            }
                            else
                            {
                                MsgPly("^9 " + Username + " wasn't found on database", MSO.UCID);
                            }
                        }

                        #endregion
                    }
                    catch
                    {
                        MsgPly("^9 An Error has occured. Please retype the Command!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^9 Not Authorized User!", MSO.UCID);
                    //AdmBox("> " + Conn.PlayerName + " (" + Conn.Username + ") tried to access the remove vehicle Command!");
                    MsgAll("^9 " + Conn.PlayerName + " tried to use remove vehicle Command!");
                }
            }
            else
            {
                MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        #endregion
    }
}
