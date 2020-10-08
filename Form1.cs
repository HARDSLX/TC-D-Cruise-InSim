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
    public partial class Form1 : Form
    {
        
        // Main InSim object
        public InSimInterface InSim;
        const string Database = @"users";

        // Note - do NOT change the settings here. Use the settings.ini file instead!
        string AdminPW = "";
        ushort Port = 54444;
        string IPAddress = "127.0.0.1";
        string HostName = "";
        byte GameMode = 2; // 0 if Demo, 1 if S1 and 2 if S2 [Automatic on IS_VER Packet]
        string InSimVer = "v1.6";

        // Team Info
        string TSIP = "ts3.servegame.com";
        string Website = "www.emirhanpala.blogspot.com.tr";
        string CruiseName = "^1[M-TeCh]^7 City Driving";
        string SeniorTag = "^7[^0iC^7]";
        string JuniorTag = "^7[^0iC^7]";
        string OfficerTag = "^4Police ^7";
        string CadetTag = "^6[COP]^7";
        string TowTruckTag = "^3Tow ^7";

        #region ' InSim Load Forms '
        // These are the main lists that contain all Players and Connections (Being maintained automatically)
        public List<clsConnection> Connections = new List<clsConnection>();

        // Delegate for UI update (Example)
        delegate void dlgMSO(Packets.IS_MSO MSO);

        // Form constructor
        public Form1()
        {
            InitializeComponent();
            InSimConnect();	// Attempt to connect to InSim
            Console.Beep();
        }

        // Always call .Close() on application exit
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
                foreach (clsConnection C in Connections)
                {
                    if (C.FailCon == 0)
                    {
                        FileInfo.SaveUser(C);
                    }
                }
                MsgAll("^9 InSim was Stopped");
                Console.Beep();
                InSim.Close();
                Application.Exit();
        }
        
        // Use this method to connect to InSim so you are able to catch any exception that might occur
        public void InSimConnect()
        {
            try
            {
                #region ' Read Settings.ini '
                if (System.IO.File.Exists(@"settings.ini") == false)
                {
                    File.Create(@"settings.ini");
                }
                StreamReader Sr = new StreamReader("settings.ini");

                string line = null;
                while ((line = Sr.ReadLine()) != null)
                {
                    if (line.Substring(0, 5) == "Admin")
                    {

                        string[] StrMsg = line.Split('=');
                        AdminPW = StrMsg[1].Trim();
                    }

                    if (line.Substring(0, 4) == "Port")
                    {
                        string[] StrMsg = line.Split('=');
                        Port = ushort.Parse(StrMsg[1].Trim());
                    }

                    if (line.Substring(0, 2) == "IP")
                    {
                        string[] StrMsg = line.Split('=');
                        IPAddress = StrMsg[1].Trim();
                    }
                    if (line.Substring(0, 5) == "HName")
                    {
                        string[] StrMsg = line.Split('=');
                        //HostName = StrMsg[1].Trim();
                    }
                }
                Sr.Close();
                #endregion

                

                // InSim connection settings
                InSimSettings Settings = new InSimSettings(IPAddress, Port, 0, Flags.InSimFlags.ISF_MSO_COLS | Flags.InSimFlags.ISF_MCI | Flags.InSimFlags.ISF_CON, '!', 500, AdminPW, JuniorTag, 5);
                InSim = new InSimInterface(Settings);	// Initialize a new instance of InSimInterface with the settings specified above
                InSim.ConnectionLost += new InSimInterface.ConnectionLost_EventHandler(LostConnectionToInSim);	// Occurs when connection was lost due to an unknown reason
                InSim.Reconnected += new InSimInterface.Reconnected_EventHandler(ReconnectedToInSim);			// Occurs when connection was recovert automatically

                InitializeInSimEvents();				// Initialize packet receive events
                InSim.Connect();						// Attempt to connect to the InSim host 
            }
            catch { }
            finally
            {
                if (InSim.State == LFS_External.InSim.InSimInterface.InSimState.Connected)
                {
                    InSim.Request_NCN_AllConnections(255);
                    InSim.Request_NPL_AllPlayers(255);
                    InSim.Send_MST_Message("/wind=1");
                    InSim.Send_MST_Message("/wind=0");
                    foreach (clsConnection C in Connections)
                    {
                        ClearPen(C.Username);
                    }
                }
            }
        }

        // Occurs when connection was lost due to an unknown reason
        private void LostConnectionToInSim()
        {
            foreach (clsConnection C in Connections)
            {
                if (C.FailCon == 0)
                {
                    FileInfo.SaveUser(C);
                }
            }
            Console.Beep();
            MsgAll("^9 InSim has been lost connection to host and now Reconnecting.");
            
            InSim.Close();
            InSimConnect();
        }

        // Occurs when connection was recovert automatically
        private void ReconnectedToInSim()
        {

        }

        // You should only enable the events you need to gain maximum performance. All events are enable by default.
        private void InitializeInSimEvents()
        {
            try
            {
                // Client information
                InSim.NCN_Received += new LFS_External.InSim.InSimInterface.NCN_EventHandler(NCN_ClientJoinsHost);				// A new client joined the server.
                InSim.CNL_Received += new LFS_External.InSim.InSimInterface.CNL_EventHandler(CNL_ClientLeavesHost);				// A client left the server.
                InSim.CPR_Received += new LFS_External.InSim.InSimInterface.CPR_EventHandler(CPR_ClientRenames);				// A client changed name or plate.
                InSim.PLP_Received += new LFS_External.InSim.InSimInterface.PLP_EventHandler(PLP_PlayerGoesToGarage);			// A player goes to the garage (setup screen).
                InSim.NPL_Received += new LFS_External.InSim.InSimInterface.NPL_EventHandler(NPL_PlayerJoinsRace);				// A player join the race. If PLID already exists, then player leaves pit.
                InSim.TOC_Received += new LFS_External.InSim.InSimInterface.TOC_EventHandler(TOC_PlayerCarTakeOver);			// Car got taken over by an other player
                InSim.PIT_Received += new LFS_External.InSim.InSimInterface.PIT_EventHandler(PIT_PlayerStopsAtPit);				// A player stops for making a pitstop
                InSim.PLL_Received += new LFS_External.InSim.InSimInterface.PLL_EventHandler(PLL_PlayerLeavesRace);				// A player leaves the race (spectate)
                InSim.BFN_Received += new LFS_External.InSim.InSimInterface.BFN_EventHandler(BFN_PlayerRequestsButtons);		// A player pressed Shift+I or Shift+B
                InSim.BTC_Received += new LFS_External.InSim.InSimInterface.BTC_EventHandler(BTC_ButtonClicked);				// A player clicked a custom button
                InSim.BTT_Received += new LFS_External.InSim.InSimInterface.BTT_EventHandler(BTT_TextBoxOkClicked);				// A player submitted a custom textbox

                // Host and race information
                InSim.STA_Received += new LFS_External.InSim.InSimInterface.STA_EventHandler(STA_StateChanged);					// The server/race state changed
                InSim.MPE_Received += new LFS_External.InSim.InSimInterface.MPE_EventHandler(MPE_MultiplayerEnd);				// A host ends or leaves
                InSim.RST_Received += new LFS_External.InSim.InSimInterface.RST_EventHandler(RST_RaceStart);					// A race starts
                InSim.VTN_Received += new LFS_External.InSim.InSimInterface.VTN_EventHandler(VTN_VoteNotify);					// A vote got called
                InSim.VTC_Received += new LFS_External.InSim.InSimInterface.VTC_EventHandler(VTC_VoteCanceled);					// A vote got canceled
                InSim.CPP_Received += new LFS_External.InSim.InSimInterface.CPP_EventHandler(CPP_CameraPosition);				// LFS reporting camera position and state
            
                // Car tracking
                InSim.MCI_Received += new LFS_External.InSim.InSimInterface.MCI_EventHandler(MCI_CarInformation);				// Detailed car information packet (max 8 per packet)

                //Car Contact
                InSim.CON_Received += new LFS_External.InSim.InSimInterface.CON_EventHandler(CON_CarContact);

                // Other
                InSim.MSO_Received += new LFS_External.InSim.InSimInterface.MSO_EventHandler(MSO_MessageOut);					// Player chat and system messages.
                InSim.VER_Received += new LFS_External.InSim.InSimInterface.VER_EventHandler(VER_InSimVersionInformation);		// InSim version information
            }
            catch { }
        }
        #endregion

        #region ' Utils '
        // Methods for automatically update Players[] and Connection[] lists
        private void RemoveFromConnectionsList(byte ucid)
        {
            // Copy of item to remove
            clsConnection RemoveItem = new clsConnection();

            // Check what item the connection had
            foreach (clsConnection Conn in Connections)
            {
                if (ucid == Conn.UniqueID)
                {
                    // Copy item (Can't delete it here)
                    RemoveItem = Conn;
                    continue;
                }
            }

            // Remove item
            Connections.Remove(RemoveItem);
        }

        private void AddToConnectionsList(Packets.IS_NCN NCN)
        {
            bool InList = false;

            // Check of connection is already in the list
            foreach (clsConnection Conn in Connections)
            {
                if (Conn.UniqueID == NCN.UCID)
                {
                    InList = true;
                    continue;
                }
            }

            // If not, add it
            if (!InList)
            {
                try
                {
                    // Assign values of new connnnection.
                    clsConnection NewConn = new clsConnection();

                    NewConn.UniqueID = NCN.UCID;
                    NewConn.Username = NCN.UName;
                    NewConn.PlayerName = NCN.PName;
                    NewConn.IsAdmin = NCN.Admin;
                    NewConn.Flags = NCN.Flags;
                    if (NewConn.Username != "")
                    {
                        #region ' File Global '
                        // Your Code File Here! init
                        NewConn.Cash = FileInfo.GetUserCash(NCN.UName);
                        NewConn.BankBalance = FileInfo.GetUserBank(NCN.UName);
                        NewConn.Cars = FileInfo.GetUserCars(NCN.UName);
                        NewConn.TotalHealth = FileInfo.GetUserHealth(NCN.UName);
                        NewConn.TotalDistance = FileInfo.GetUserDistance(NCN.UName);
                        NewConn.TotalJobsDone = FileInfo.GetUserJobsDone(NCN.UName);

                        NewConn.Electronics = FileInfo.GetUserElectronics(NCN.UName);
                        NewConn.Furniture = FileInfo.GetUserFurniture(NCN.UName);

                        NewConn.IsSuperAdmin = FileInfo.GetUserAdmin(NCN.UName);
                        
                        NewConn.IsModerator = FileInfo.IsMember(NCN.UName);
                        NewConn.CanBeOfficer = FileInfo.CanBeOfficer(NCN.UName);
                        NewConn.CanBeCadet = FileInfo.CanBeCadet(NCN.UName);
                        NewConn.CanBeTowTruck = FileInfo.CanBeTowTruck(NCN.UName);

                        NewConn.Interface = FileInfo.GetInterface(NCN.UName);
                        NewConn.InGameIntrfc = FileInfo.GetInterface2(NCN.UName);
                        NewConn.KMHorMPH = FileInfo.GetSpeedo(NCN.UName);
                        NewConn.Odometer = FileInfo.GetOdometer(NCN.UName);
                        NewConn.Counter = FileInfo.GetCounter(NCN.UName);
                        NewConn.CopPanel = FileInfo.GetCopPanel(NCN.UName);

                        NewConn.LastRaffle = FileInfo.GetUserLastRaffle(NCN.UName);
                        NewConn.LastLotto = FileInfo.GetUserLastLotto(NCN.UName);

                        NewConn.Renting = FileInfo.GetUserRenting(NCN.UName);
                        NewConn.Rentee = FileInfo.GetUserRentee(NCN.UName);
                        NewConn.Renter = FileInfo.GetUserRenter(NCN.UName);
                        NewConn.Renterr = FileInfo.GetUserRenterr(NCN.UName);
                        NewConn.Rented = FileInfo.GetUserRented(NCN.UName);
                        #endregion
                    }
                    else
                    {
                        #region ' Hoster '
                        NewConn.Cash = 0;
                        NewConn.BankBalance = 0;
                        NewConn.Cars = "UF1";
                        NewConn.TotalHealth = 100;
                        NewConn.TotalDistance = 0;
                        NewConn.TotalJobsDone = 0;

                        NewConn.Electronics = 0;
                        NewConn.Furniture = 0;

                        NewConn.IsSuperAdmin = 0;

                        NewConn.IsModerator = 0;
                        NewConn.CanBeOfficer = 0;
                        NewConn.CanBeCadet = 0;
                        NewConn.CanBeTowTruck = 0;

                        NewConn.Interface = 3;
                        NewConn.InGameIntrfc = 3;
                        NewConn.KMHorMPH = 3;
                        NewConn.Odometer = 3;
                        NewConn.Counter = 3;
                        NewConn.CopPanel = 3;

                        NewConn.LastRaffle = 0;
                        NewConn.LastLotto = 0;


                        NewConn.FailCon = 1;
                        #endregion
                    }
                    NewConn.BankBonusTimer = 3600;
                    
                    NewConn.Chasee = -1;
                    NewConn.IsOfficer = false;
                    NewConn.IsCadet = false;
                    NewConn.IsSuspect = false;
                    NewConn.InChaseProgress = false;
                    NewConn.AutoBumpTimer = 0;
                    NewConn.ChaseCondition = 0;
                    NewConn.BumpButton = 0;
                    NewConn.BustedTimer = 0;
                    NewConn.Busted = false;
                    NewConn.IsBeingBusted = false;

                    NewConn.Towee = -1;
                    NewConn.InTowProgress = false;
                    NewConn.IsBeingTowed = false;

                    NewConn.InModerationMenu = 0;
                    NewConn.ModReason = "";
                    NewConn.ModReasonSet = false;
                    NewConn.ModUsername = "";
                    NewConn.ModerationWarn = 0;

                    Connections.Add(NewConn);
                }
                catch
                {
                    clsConnection NewConn = new clsConnection();
                    NewConn.PlayerName = NCN.PName;
                    NewConn.Username = NCN.UName;
                    NewConn.FailCon = 1;
                    Connections.Add(NewConn);
                    InSim.Send_MST_Message("/kick " + NewConn.Username);
                }
            }
        }

        #region ' PLID and UCID Identifier '
        /// <summary>
        /// Returns an index value for Connections[] that corresponds with the UniqueID of a connection
        /// </summary>
        /// <param name="UNID">UCID to find</param>
        public int GetConnIdx(int UNID)
        {
            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i].UniqueID == UNID) { return i; }
            }
            return 0;
        }
        public int GetConnIdx2(int PLID)
        {
            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i].PlayerID == PLID) { return i; }
            }
            return 0;
        }
        
        /// <summary>Returns true if method needs invoking due to threading</summary>
        private bool DoInvoke()
        {
            foreach (Control c in this.Controls)
            {
                if (c.InvokeRequired) return true;
                break;	// 1 control is enough
            }
            return false;
        }
        #endregion

        #endregion

        #region ' Packet receive events '

        // Player chat and system messages.
        private void MSO_MessageOut(Packets.IS_MSO MSO)
        {
            try
            {
                // Invoke method due to threading. Add this line to any receive event before updating the GUI. Just like in this example, you only have to add a new delegate with the right packet parameter and adjust this line in the new method.
                if (DoInvoke()) { object p = MSO; this.Invoke(new dlgMSO(MSO_MessageOut), p); return; }

                var Conn = Connections[GetConnIdx(MSO.UCID)];
                string Msg = MSO.Msg.Substring(MSO.TextStart, (MSO.Msg.Length - MSO.TextStart));
                string[] StrMsg = Msg.Split(' ');

                

                #region ' Spam and Swear Check '

                

                #region ' Spam Filter '
                // Spam Filter
                if (Msg.StartsWith("!") == false)
                {
                    if (MSO.UCID == 0 == false && Conn.PlayerName == HostName == false)
                    {
                        Conn.Spam += 1;
                        Conn.SpamTime = 8;
                        if (Conn.Spam == 4)
                        {
                            Conn.Cash -= 700;
                            MsgAll("^9 " + Conn.PlayerName + " ^7was fined ^1$700 ^7for spamming");
                        }
                        else if (Conn.Spam == 7)
                        {
                            Conn.Cash -= 700;
                            MsgAll("^9 " + Conn.PlayerName + " ^7was fined ^1$700 ^7and kicked for spamming");
                            KickID(Conn.Username);
                        }
                    }
                }
                #endregion

                #endregion

                #region ' Penalty Check '
                if (MSO.Msg.Contains(" : DRIVE-THROUGH PENALTY"))
                {
                    if (Conn.PlayerName == HostName)
                    {
                        string Name;

                        Name = Msg;
                        Name = Name.Replace(" : DRIVE-THROUGH PENALTY", "");
                        foreach (clsConnection C in Connections)
                        {
                            if (Name.Contains(C.PlayerName))
                            {
                                int RandomFines = new Random().Next(500, 700);
                                MsgAll("^9 " + C.PlayerName + " was fined ^1$" + RandomFines + " ^7for speeding.");
                                C.Cash -= RandomFines;
                                C.Penalty = 8;
                            }
                        }
                    }
                }

                if (MSO.Msg.Contains(" : STOP-GO PENALTY"))
                {
                    if (Conn.PlayerName == HostName)
                    {
                        string Name;

                        Name = Msg;
                        Name = Name.Replace(" : STOP-GO PENALTY", "");
                        foreach (clsConnection C in Connections)
                        {
                            if (Name.Contains(C.PlayerName))
                            {
                                int RandomFines = new Random().Next(500, 700);
                                MsgAll("^9 " + C.PlayerName + " was fined ^1$" + RandomFines + " ^7and spected for speeding.");
                                SpecID(C.PlayerName);
                                SpecID(C.Username);
                                C.Cash -= RandomFines;
                                C.Penalty = 0;
                            }
                        }
                    }
                }
                #endregion

                #region ' Chat command '
                if (Msg.StartsWith("!"))
                {
                    string cmp = StrMsg[0].Remove(0, 1);
                    if (Conn.WaitCMD == 0)
                    {
                        foreach (CommandList CL in Commands)
                        {
                            if (cmp == CL.CommandArg.Command)
                            {
                                CL.MethodInf.Invoke(this, new object[] { Msg, StrMsg, MSO });
                                return;
                            }
                        }
                        MsgPly("^9 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                        //Conn.WaitCMD = 4;
                    }
                    else
                    {
                        MsgPly("^9 You have to wait ^2" + Conn.WaitCMD + " ^7second(s) to start command", MSO.UCID);
                    }
                }
                #endregion
            }
            catch { }
        }

        // A new client joined the server.
        private void NCN_ClientJoinsHost(Packets.IS_NCN NCN)
        {
            try
            {
                FileInfo.NewCruiser(NCN.UName, NCN.PName);
                AddToConnectionsList(NCN);                  // Update Connections[] list (don't remove this line!)
                var Conn = Connections[GetConnIdx(NCN.UCID)];
                //InSim.Send_BTN_CreateButton("VERSION: " + InSimVer, Flags.ButtonStyles.ISB_C1, 4, 35, 0, 172, 0, Conn.UniqueID, 2, true);

                ClearPen(NCN.UName);

                #region ' Special PlayerName Colors Remove '
                Conn.PlayerName = NCN.PName;
                if (Conn.PlayerName.Contains("^0"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^0", "");
                }
                if (Conn.PlayerName.Contains("^1"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^1", "");
                }
                if (Conn.PlayerName.Contains("^2"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^2", "");
                }
                if (Conn.PlayerName.Contains("^3"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^3", "");
                }
                if (Conn.PlayerName.Contains("^4"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^4", "");
                }
                if (Conn.PlayerName.Contains("^5"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^5", "");
                }
                if (Conn.PlayerName.Contains("^6"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^6", "");
                }
                if (Conn.PlayerName.Contains("^7"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^7", "");
                }
                if (Conn.PlayerName.Contains("^8"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^8", "");
                }
                #endregion

                #region ' Get User Permanent Ban '

                if (FileInfo.GetUserPermBan(NCN.UName.ToLower()) == 1)
                {
                    Message("/msg ^9 " + Conn.PlayerName + " is on the ban list.");
                   
                    BanID(Conn.Username, 0);
                }

                #endregion

                #region ' Retrieve HostName '
                if (NCN.UCID == 0 && NCN.UName == "")
                {
                    HostName = NCN.PName;
                }

                #region ' Check Player Name if contains host '
                if (NCN.UCID == 0 && NCN.UName == "" && NCN.PName == HostName == false)
                {
                    if (NCN.PName.Contains(HostName))
                    {
                        MsgAll("^9 " + Conn.PlayerName + " was kicked for having Host Name!");
                        KickID(NCN.UName);
                    }
                }
                #endregion

                #endregion

                #region ' Check Tags of Team '

                if (NCN.PName == HostName == false)
                {
                    if (NCN.Admin == 1)
                    {
                        if (Conn.IsSuperAdmin == 1)
                        {
                           
                        }
                        else
                        {
                            MsgAll("^9 " + Conn.PlayerName + " is not a admin!");
                            ///AdmBox("> " + Conn.PlayerName + " (" + NCN.UName + ") tried to access as Admin! but kicked.");
                            KickID(NCN.UName);
                        }
                    }
                }

                if (NCN.PName.Contains(SeniorTag) || NCN.PName.Contains(JuniorTag))
                {
                    if (Conn.IsModerator == 0 && Conn.IsSuperAdmin == 0 && Conn.IsAdmin == 0)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " is not a Moderator/Admin!");
                        MsgPly("^9 Please Remove the tag and come back!", NCN.UCID);
                        KickID(NCN.UName);
                    }
                }

                if (NCN.PName.Contains(OfficerTag))
                {
                    if (Conn.CanBeOfficer == 0)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " is not a Officer!");
                        MsgPly("^9 Please Remove the tag and come back!", NCN.UCID);
                        KickID(NCN.UName);
                    }
                }

                if (NCN.PName.Contains(CadetTag))
                {
                    if (Conn.CanBeCadet == 0)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " is not a Cadet!");
                        MsgPly("^9 Please Remove the tag!", NCN.UCID);
                        KickID(NCN.UName);
                    }
                    else if (Conn.CanBeCadet == 2)
                    {
                        MsgPly("^9 Use " + OfficerTag + " when going duty!", NCN.UCID);
                    }
                    else if (Conn.CanBeCadet == 3)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " is not a Cadet!");
                        MsgPly("^9 Please Remove the tag!", NCN.UCID);
                        MsgPly("^9 You are already revoked as Cadet!", NCN.UCID);
                        KickID(NCN.UName);
                    }
                }

                if (NCN.PName.Contains(TowTruckTag))
                {
                    if (Conn.CanBeTowTruck == 0)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " is not a TowTruck!");
                        MsgPly("^9 Please Remove the tag!", NCN.UCID);
                        KickID(NCN.UName);
                    }
                }
                #endregion

                Conn.Location = "Spectators";
                Conn.LastSeen = "Spectators, In Game";
                Conn.LocationBox = "^7Spectators";
                Conn.SpeedBox = "";
            }
            catch { }
        }

        // A client left the server.
        private void CNL_ClientLeavesHost(Packets.IS_CNL CNL)
        {
            try
            {
                #region ' variables '

                clsConnection Conn = Connections[GetConnIdx(CNL.UCID)];
                clsConnection ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(CNL.UCID)].Chasee)];
                clsConnection TowCon = Connections[GetConnIdx(Connections[GetConnIdx(CNL.UCID)].Towee)];
                #endregion

                #region ' In Game Neccesities '
                if (Conn.InGame == 1)
                {
                    #region ' Bonus Done Region '
                    if (Conn.TotalBonusDone == 0)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 400 + "%");
                    }
                    else if (Conn.TotalBonusDone == 1)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 800 + "%");
                    }
                    else if (Conn.TotalBonusDone == 2)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 3)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 1200 + "%");
                    }
                    else if (Conn.TotalBonusDone == 4)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 1600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 5)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 2000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 6)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 2600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 7)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 3200 + "%");
                    }
                    else if (Conn.TotalBonusDone == 8)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 4000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 9)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 5400 + "%");
                    }
                    else if (Conn.TotalBonusDone == 10)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 6600 + "%");
                    }
                    #endregion

                    #region ' OnScreen Spectators '
                    if (Conn.WaitIntrfc == 0)
                    {
                        if (Conn.Interface == 0)
                        {
                            DeleteBTN(8, Conn.UniqueID);
                            InSim.Send_BTN_CreateButton("^7Spectators", Flags.ButtonStyles.ISB_DARK, 5, 25, 86, 172, 9, Conn.UniqueID, 2, false);
                        }
                    }
                    #endregion

                    #region ' On Screen PitLane Clear '
                    if (Conn.LeavesPitLane == 1)
                    {
                        Conn.LeavesPitLane = 0;
                    }
                    if (Conn.OnScreenExit > 0)
                    {
                        DeleteBTN(10, Conn.UniqueID);
                        Conn.OnScreenExit = 0;
                    }

                    #endregion

                    #region ' OnScreen Ahead '

                    if (Conn.StreetSign > 0)
                    {
                        DeleteBTN(11, Conn.UniqueID);
                        DeleteBTN(12, Conn.UniqueID);
                        DeleteBTN(13, Conn.UniqueID);
                        Conn.StreetSign = 0;
                    }

                    #endregion

                    #region ' OnScreen Sign '

                    if (Conn.MapSignActivated == true)
                    {
                        if (Conn.MapSigns > 0)
                        {
                            DeleteBTN(10, Conn.UniqueID);
                            Conn.MapSigns = 0;
                        }
                    }

                    #endregion

                    

                    #region ' Remove Cop Panel '
                    if (Conn.IsOfficer == true || Conn.IsCadet == true)
                    {
                        #region ' Remove Cop Panel '

                        DeleteBTN(15, Conn.UniqueID);
                        DeleteBTN(16, Conn.UniqueID);
                        DeleteBTN(17, Conn.UniqueID);
                        DeleteBTN(18, Conn.UniqueID);
                        DeleteBTN(19, Conn.UniqueID);
                        DeleteBTN(20, Conn.UniqueID);
                        DeleteBTN(21, Conn.UniqueID);
                        DeleteBTN(22, Conn.UniqueID);

                        #endregion
                    }
                    #endregion

                    #region ' Close BTN '
                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                    {
                        if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true || Conn.InSchool == true || Conn.InShop == true || Conn.InStore == true || Conn.InBank == true)
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
                        }
                        Conn.DisplaysOpen = false;
                    }
                    #endregion

                    #region ' Close Location '

                    if (Conn.InBank == true)
                    {
                        Conn.InBank = false;
                    }
                    if (Conn.InHouse1 == true)
                    {
                        Conn.InHouse1 = false;
                    }
                    if (Conn.InHouse2 == true)
                    {
                        Conn.InHouse2 = false;
                    }
                    if (Conn.InHouse3 == true)
                    {
                        Conn.InHouse3 = false;
                    }
                    if (Conn.InSchool == true)
                    {
                        Conn.InSchool = false;
                    }

                    if (Conn.InShop == true)
                    {
                        Conn.InShop = false;
                    }
                    if (Conn.InStore == true)
                    {
                        Conn.InStore = false;
                    }
                    #endregion

                    Conn.TotalBonusDone = 0;
                    Conn.BonusDistance = 0;
                    Conn.InGame = 0;
                }
                #endregion

                #region ' Cop System '

                if (Conn.TrapSetted == true)
                {
                    MsgPly("^9 Speed Trap Removed", Conn.UniqueID);
                    Conn.TrapY = 0;
                    Conn.TrapX = 0;
                    Conn.TrapSpeed = 0;
                    Conn.TrapSetted = false;
                }

                if (Conn.IsSuspect == true)
                {
                    MsgAll("^9 " + Conn.PlayerName + " was fined ^1$5000");
                    MsgAll("  ^7For specting on track whilst being chased!");
                    Conn.Cash -= 5000;

                    #region ' In Connection List '
                    foreach (clsConnection i in Connections)
                    {
                        if (i.Chasee == Conn.UniqueID)
                        {
                            if (i.JoinedChase == true)
                            {
                                i.JoinedChase = false;
                            }
                            i.BustedTimer = 0;
                            i.Busted = false;
                            i.AutoBumpTimer = 0;
                            i.BumpButton = 0;
                            i.ChaseCondition = 0;
                            i.InChaseProgress = false;
                            i.Chasee = -1;
                        }
                    }
                    #endregion

                    AddChaseLimit -= 1;
                    Conn.PullOvrMsg = 0;
                    Conn.ChaseCondition = 0;
                    Conn.CopInChase = 0;
                    Conn.IsSuspect = false;
                    CopSirenShutOff();
                }

                if (Conn.InFineMenu == true)
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

                if (Conn.IsBeingBusted == true)
                {
                    MsgAll("^9 " + Conn.PlayerName + " was fined ^1$5000");
                    MsgAll("  ^7For specting on track whilst being busted!");
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
                                MsgAll("^9 " + i.PlayerName + " was rewarded for ^2$" + (Convert.ToInt16(5000 * 0.2)));
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

                    Conn.PullOvrMsg = 0;
                    Conn.ChaseCondition = 0;
                    Conn.AcceptTicket = 0;
                    Conn.TicketRefuse = 0;
                    Conn.CopInChase = 0;
                    Conn.IsBeingBusted = false;
                }

                if (Conn.AcceptTicket == 2)
                {
                    #region ' Connection List '
                    foreach (clsConnection i in Connections)
                    {
                        if (i.Chasee == Conn.UniqueID)
                        {
                            if (i.InFineMenu == true)
                            {
                                i.InFineMenu = false;
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

                    Conn.AcceptTicket = 0;
                    Conn.IsBeingBusted = false;
                    Conn.CopInChase = 0;
                    Conn.TicketRefuse = 0;

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
                }

                if (Conn.InChaseProgress == true)
                {
                    if (ChaseCon.CopInChase > 1)
                    {
                        if (Conn.JoinedChase == true)
                        {
                            Conn.JoinedChase = false;
                        }
                        Conn.ChaseCondition = 0;
                        Conn.Busted = false;
                        Conn.BustedTimer = 0;
                        Conn.BumpButton = 0;
                        Conn.Chasee = -1;
                        ChaseCon.CopInChase -= 1;

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

                        MsgAll("^9 " + Conn.PlayerName + " sighting lost " + ChaseCon.PlayerName + "!");
                        MsgAll("   ^7 Total Cops In Chase: " + ChaseCon.CopInChase);
                    }
                    else if (ChaseCon.CopInChase == 1)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " lost " + ChaseCon.PlayerName + "!");
                        MsgAll("   ^7Suspect Runs away from being chased!");
                        AddChaseLimit -= 1;
                        Conn.AutoBumpTimer = 0;
                        Conn.BumpButton = 0;
                        Conn.BustedTimer = 0;
                        Conn.Chasee = -1;
                        Conn.Busted = false;
                        ChaseCon.PullOvrMsg = 0;
                        ChaseCon.ChaseCondition = 0;
                        ChaseCon.CopInChase = 0;
                        ChaseCon.IsSuspect = false;
                        Conn.ChaseCondition = 0;
                        CopSirenShutOff();
                    }

                    Conn.InChaseProgress = false;
                }

                #endregion

                #region ' Tow System '

                if (Conn.InTowProgress == true)
                {
                    if (TowCon.IsBeingTowed == true)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " stopped towing " + TowCon.PlayerName + "!");
                        TowCon.IsBeingTowed = false;
                    }
                    Conn.Towee = -1;
                    Conn.InTowProgress = false;
                    CautionSirenShutOff();
                }

                if (Conn.IsBeingTowed == true)
                {
                    MsgAll("^9 " + Conn.PlayerName + " leaves whilst being Towed!");
                    foreach (clsConnection o in Connections)
                    {
                        if (o.Towee == Conn.UniqueID)
                        {
                            o.InTowProgress = false;
                            o.Towee = -1;
                        }
                    }
                    Conn.IsBeingTowed = false;
                    CautionSirenShutOff();
                }

                #endregion

                #region ' Return Rent '
                if (Conn.Rented == 1)
                {
                    bool Found = false;

                    #region ' Online '
                    foreach (clsConnection C in Connections)
                    {
                        if (C.Username == Conn.Rentee)
                        {
                            Found = true;
                            C.Renting = 0;
                            C.Renter = "0";
                            MsgAll("^9 " + Conn.PlayerName + " their rentals returned to " + C.PlayerName);
                        }
                    }
                    #endregion

                    #region ' Offline '
                    if (Found == false)
                    {
                        #region ' Objects '

                        long Cash = FileInfo.GetUserCash(Conn.Rentee);
                        long BBal = FileInfo.GetUserBank(Conn.Rentee);
                        string Cars = FileInfo.GetUserCars(Conn.Rentee);
                        long Gold = FileInfo.GetUserGold(Conn.Rentee);

                        long TotalDistance = FileInfo.GetUserDistance(Conn.Rentee);
                        byte TotalHealth = FileInfo.GetUserHealth(Conn.Rentee);
                        int TotalJobsDone = FileInfo.GetUserJobsDone(Conn.Rentee);

                        byte Electronics = FileInfo.GetUserElectronics(Conn.Rentee);
                        byte Furniture = FileInfo.GetUserFurniture(Conn.Rentee);

                        int LastRaffle = FileInfo.GetUserLastRaffle(Conn.Rentee);
                        int LastLotto = FileInfo.GetUserLastLotto(Conn.Rentee);

                        byte CanBeOfficer = FileInfo.CanBeOfficer(Conn.Rentee);
                        byte CanBeCadet = FileInfo.CanBeCadet(Conn.Rentee);
                        byte CanBeTowTruck = FileInfo.CanBeTowTruck(Conn.Rentee);
                        byte IsModerator = FileInfo.IsMember(Conn.Rentee);

                        byte Interface1 = FileInfo.GetInterface(Conn.Rentee);
                        byte Interface2 = FileInfo.GetInterface2(Conn.Rentee);
                        byte Speedo = FileInfo.GetSpeedo(Conn.Rentee);
                        byte Odometer = FileInfo.GetOdometer(Conn.Rentee);
                        byte Counter = FileInfo.GetCounter(Conn.Rentee);
                        byte Panel = FileInfo.GetCopPanel(Conn.Rentee);

                        byte Renting = FileInfo.GetUserRenting(Conn.Rentee);
                        byte Rented = FileInfo.GetUserRented(Conn.Rentee);
                        string Renter = FileInfo.GetUserRenter(Conn.Rentee);
                        string Renterr = FileInfo.GetUserRenterr(Conn.Rentee);
                        string Rentee = FileInfo.GetUserRentee(Conn.Rentee);

                        string PlayerName = FileInfo.GetUserPlayerName(Conn.Rentee);
                        #endregion

                        #region ' Remove Renting '

                        Renting = 0;
                        Renter = "0";

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

                        MsgAll("^9 " + Conn.PlayerName + " their rentals returned to " + PlayerName);

                        #region ' Save User '

                        FileInfo.SaveOfflineUser(Conn.Rentee,
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
                    #endregion

                    Conn.Rentee = "0";
                    Conn.Rented = 0;
                }
                #endregion

                

                

                RemoveFromConnectionsList(CNL.UCID);		// Update Connections[] list
            }
            catch { }
        }

        // A client changed name or plate.
        private void CPR_ClientRenames(Packets.IS_CPR CPR)
        {
            try
            {
                clsConnection Conn = Connections[GetConnIdx(CPR.UCID)];
                clsConnection ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(CPR.UCID)].Chasee)];
                clsConnection TowCon = Connections[GetConnIdx(Connections[GetConnIdx(CPR.UCID)].Towee)];
                #region ' Special PlayerName Colors Remove '
                Conn.PlayerName = CPR.PName;
                if (Conn.PlayerName.Contains("^0"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^0", "");
                }
                if (Conn.PlayerName.Contains("^1"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^1", "");
                }
                if (Conn.PlayerName.Contains("^2"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^2", "");
                }
                if (Conn.PlayerName.Contains("^3"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^3", "");
                }
                if (Conn.PlayerName.Contains("^4"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^4", "");
                }
                if (Conn.PlayerName.Contains("^5"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^5", "");
                }
                if (Conn.PlayerName.Contains("^6"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^6", "");
                }
                if (Conn.PlayerName.Contains("^7"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^7", "");
                }
                if (Conn.PlayerName.Contains("^8"))
                {
                    Conn.PlayerName = Conn.PlayerName.Replace("^8", "");
                }
                #endregion

                

                #region ' Check Officer/Cadet Names '

                #region ' Officer '
                if (CPR.PName.Contains(OfficerTag))
                {
                    if (Conn.CanBeOfficer == 1)
                    {
                        if (Conn.JobToHouse1 == false && Conn.JobToHouse2 == false && Conn.JobToHouse3 == false && Conn.JobToSchool == false)
                        {
                            if (CPR.Plate == "Police")
                            {
                                #region ' Duty Officer '
                                if (Conn.IsOfficer == false)
                                {
                                    MsgAll("^9 " + Conn.PlayerName + " is now ^2DUTY ^7as Officer!");

                                    if (Conn.CopPanel == 0)
                                    {
                                        MsgPly("^9 Your Panel Click is disabled", CPR.UCID);
                                        MsgPly("  ^7To Enable them by typing ^2!coppanel", CPR.UCID);
                                    }
                                    else if (Conn.CopPanel == 1)
                                    {
                                        MsgPly("^9 Your Panel Click is enabled", CPR.UCID);
                                        MsgPly("  ^7To Disable them by typing ^2!coppanel", CPR.UCID);
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
                                #endregion
                            }
                            else
                            {
                                #region ' Remove '
                                if (Conn.InChaseProgress == true)
                                {
                                    MsgAll("^9 " + Conn.PlayerName + " ends chase on " + ChaseCon.PlayerName + "!");

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
                                        ChaseCon.ChaseCondition = 0;
                                        ChaseCon.CopInChase = 0;
                                        ChaseCon.IsSuspect = false;
                                        Conn.ChaseCondition = 0;
                                        CopSirenShutOff();
                                    }
                                    #endregion

                                    #region ' Remove Cop Panel '

                                    DeleteBTN(15, Conn.UniqueID);
                                    DeleteBTN(16, Conn.UniqueID);
                                    DeleteBTN(17, Conn.UniqueID);
                                    DeleteBTN(18, Conn.UniqueID);
                                    DeleteBTN(19, Conn.UniqueID);
                                    DeleteBTN(20, Conn.UniqueID);
                                    DeleteBTN(21, Conn.UniqueID);
                                    DeleteBTN(22, Conn.UniqueID);

                                    #endregion

                                    #region ' Restore some BTN '
                                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                    {
                                        if (Conn.InShop == true)
                                        {
                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        if (Conn.InStore == true)
                                        {
                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    #endregion

                                    if (Conn.IsOfficer == true)
                                    {
                                        MsgPly("^9 Your Platenumber must be in ' Police '!", CPR.UCID);
                                        MsgAll("^9 " + Conn.LastName + " is now ^1OFF-DUTY ^7as Officer!");
                                        TotalOfficers -= 1;
                                        Conn.LastName = "";
                                        Conn.IsOfficer = false;
                                    }
                                }
                                else
                                {
                                    MsgPly("^9 Your Platenumber must be in ' Police '!", CPR.UCID);

                                    #region ' Remove Cop Panel '

                                    DeleteBTN(15, Conn.UniqueID);
                                    DeleteBTN(16, Conn.UniqueID);
                                    DeleteBTN(17, Conn.UniqueID);
                                    DeleteBTN(18, Conn.UniqueID);
                                    DeleteBTN(19, Conn.UniqueID);
                                    DeleteBTN(20, Conn.UniqueID);
                                    DeleteBTN(21, Conn.UniqueID);
                                    DeleteBTN(22, Conn.UniqueID);

                                    #endregion

                                    #region ' Restore some BTN '
                                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                    {
                                        if (Conn.InShop == true)
                                        {
                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        if (Conn.InStore == true)
                                        {
                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    #endregion

                                    if (Conn.IsOfficer == true)
                                    {
                                        MsgAll("^9 " + Conn.LastName + " is now ^1OFF-DUTY ^7as Officer!");
                                        TotalOfficers -= 1;
                                        Conn.LastName = "";
                                        Conn.IsOfficer = false;
                                    }
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            MsgPly("^9 Cancel your current job first!", CPR.UCID);
                        }
                    }
                    else
                    {
                        MsgAll("^9 " + Conn.PlayerName + " is not a Police Officer!");
                        MsgPly("^9 Please remove the tag!", CPR.UCID);
                        KickID(Conn.Username);
                    }
                }
                else
                {
                    #region ' Remove '
                    if (Conn.IsOfficer == true)
                    {
                        if (Conn.InChaseProgress == true)
                        {
                            MsgAll("^9 " + Conn.PlayerName + " ends chase on " + ChaseCon.PlayerName + "!");

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
                                ChaseCon.ChaseCondition = 0;
                                ChaseCon.CopInChase = 0;
                                ChaseCon.IsSuspect = false;
                                Conn.ChaseCondition = 0;
                                CopSirenShutOff();
                            }
                            #endregion

                            #region ' Remove Cop Panel '

                            DeleteBTN(15, Conn.UniqueID);
                            DeleteBTN(16, Conn.UniqueID);
                            DeleteBTN(17, Conn.UniqueID);
                            DeleteBTN(18, Conn.UniqueID);
                            DeleteBTN(19, Conn.UniqueID);
                            DeleteBTN(20, Conn.UniqueID);
                            DeleteBTN(21, Conn.UniqueID);
                            DeleteBTN(22, Conn.UniqueID);

                            #endregion

                            #region ' Restore some BTN '
                            if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                            {
                                if (Conn.InShop == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                                if (Conn.InStore == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                            }
                            #endregion

                            MsgAll("^9 " + Conn.LastName + " is now ^1OFF-DUTY ^7as Officer!");
                            TotalOfficers -= 1;
                            Conn.LastName = "";
                            Conn.IsOfficer = false;
                        }
                        else
                        {
                            #region ' Remove Cop Panel '

                            DeleteBTN(15, Conn.UniqueID);
                            DeleteBTN(16, Conn.UniqueID);
                            DeleteBTN(17, Conn.UniqueID);
                            DeleteBTN(18, Conn.UniqueID);
                            DeleteBTN(19, Conn.UniqueID);
                            DeleteBTN(20, Conn.UniqueID);
                            DeleteBTN(21, Conn.UniqueID);
                            DeleteBTN(22, Conn.UniqueID);

                            #endregion

                            #region ' Restore some BTN '
                            if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                            {
                                if (Conn.InShop == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                                if (Conn.InStore == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                            }
                            #endregion

                            MsgAll("^9 " + Conn.LastName + " is now ^1OFF-DUTY ^7as Officer!");
                            Conn.LastName = "";
                            TotalOfficers -= 1;
                            Conn.IsOfficer = false;
                        }
                    }
                    #endregion
                }
                #endregion

                #region ' Cadet '
                if (CPR.PName.Contains(CadetTag))
                {
                    if (Conn.CanBeCadet == 1)
                    {
                        if (Conn.JobToHouse1 == false && Conn.JobToHouse2 == false && Conn.JobToHouse3 == false && Conn.JobToSchool == false)
                        {
                            if (CPR.Plate == "Police")
                            {
                                #region ' Duty Cadet '
                                if (Conn.IsCadet == false)
                                {
                                    MsgAll("^9 " + Conn.PlayerName + " is now ^2DUTY ^7as Cadet!");

                                    if (Conn.CopPanel == 0)
                                    {
                                        MsgPly("^9 Your Panel Click is disabled", CPR.UCID);
                                        MsgPly("  ^7To Enable them by typing ^2!coppanel", CPR.UCID);
                                    }
                                    else if (Conn.CopPanel == 1)
                                    {
                                        MsgPly("^9 Your Panel Click is enabled", CPR.UCID);
                                        MsgPly("  ^7To Disable them by typing ^2!coppanel", CPR.UCID);
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
                                #endregion
                            }
                            else
                            {
                                #region ' Remove '
                                if (Conn.InChaseProgress == true)
                                {
                                    MsgAll("^9 " + Conn.PlayerName + " ends chase on " + ChaseCon.PlayerName + "!");

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
                                        ChaseCon.ChaseCondition = 0;
                                        ChaseCon.CopInChase = 0;
                                        ChaseCon.IsSuspect = false;
                                        Conn.ChaseCondition = 0;
                                        CopSirenShutOff();
                                    }
                                    #endregion

                                    #region ' Remove Cop Panel '

                                    DeleteBTN(15, Conn.UniqueID);
                                    DeleteBTN(16, Conn.UniqueID);
                                    DeleteBTN(17, Conn.UniqueID);
                                    DeleteBTN(18, Conn.UniqueID);
                                    DeleteBTN(19, Conn.UniqueID);
                                    DeleteBTN(20, Conn.UniqueID);
                                    DeleteBTN(21, Conn.UniqueID);
                                    DeleteBTN(22, Conn.UniqueID);

                                    #endregion

                                    #region ' Restore some BTN '
                                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                    {
                                        if (Conn.InShop == true)
                                        {
                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        if (Conn.InStore == true)
                                        {
                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    #endregion

                                    if (Conn.IsCadet == true)
                                    {
                                        MsgPly("^9 Your Platenumber must be in ' Police '!", CPR.UCID);
                                        MsgAll("^9 " + Conn.LastName + " is now ^1OFF-DUTY ^7as a Cop!");
                                        Conn.LastName = "";
                                        Conn.IsCadet = false;
                                    }
                                }
                                else
                                {
                                    MsgPly("^9 Your Platenumber must be in ' Police '!", CPR.UCID);

                                    #region ' Remove Cop Panel '

                                    DeleteBTN(15, Conn.UniqueID);
                                    DeleteBTN(16, Conn.UniqueID);
                                    DeleteBTN(17, Conn.UniqueID);
                                    DeleteBTN(18, Conn.UniqueID);
                                    DeleteBTN(19, Conn.UniqueID);
                                    DeleteBTN(20, Conn.UniqueID);
                                    DeleteBTN(21, Conn.UniqueID);
                                    DeleteBTN(22, Conn.UniqueID);

                                    #endregion

                                    #region ' Restore some BTN '
                                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                    {
                                        if (Conn.InShop == true)
                                        {
                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        if (Conn.InStore == true)
                                        {
                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    #endregion

                                    if (Conn.IsCadet == true)
                                    {
                                        MsgAll("^9 " + Conn.LastName + " is now ^1OFF-DUTY ^7as a Cop!");
                                        Conn.LastName = "";
                                        Conn.IsCadet = true;
                                    }
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            MsgPly("^9 Cancel your current job first!", CPR.UCID);
                        }
                    }
                    #region ' False or Removed Cadet '
                    else if (Conn.CanBeCadet == 2)
                    {
                        MsgPly("^9 You are already a Officer Status!", CPR.UCID);
                        MsgPly("  ^7Please use " + OfficerTag + " ^7instead of " + CadetTag, CPR.UCID);
                    }
                    else
                    {
                        #region ' Not cadet or removed '
                        if (Conn.CanBeCadet == 0)
                        {
                            MsgAll("^9 " + Conn.PlayerName + " is not a Police Cadet!");
                            MsgPly("^9 Please remove the tag!", CPR.UCID);
                            KickID(Conn.Username);
                        }
                        else if (Conn.CanBeCadet == 3)
                        {
                            MsgAll("^9 " + Conn.PlayerName + " is not a Police Cadet!");
                            MsgPly("^9 Please remove the tag!", CPR.UCID);
                            KickID(Conn.Username);
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region ' Remove '
                    if (Conn.IsCadet == true)
                    {
                        if (Conn.InChaseProgress == true)
                        {
                            MsgAll("^9 " + Conn.PlayerName + " ends chase on " + ChaseCon.PlayerName + "!");

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
                                ChaseCon.ChaseCondition = 0;
                                ChaseCon.CopInChase = 0;
                                ChaseCon.IsSuspect = false;
                                Conn.ChaseCondition = 0;
                                CopSirenShutOff();
                            }
                            #endregion

                            #region ' Remove Cop Panel '
                                DeleteBTN(15, Conn.UniqueID);
                                DeleteBTN(16, Conn.UniqueID);
                                DeleteBTN(17, Conn.UniqueID);
                                DeleteBTN(18, Conn.UniqueID);
                                DeleteBTN(19, Conn.UniqueID);
                                DeleteBTN(20, Conn.UniqueID);
                                DeleteBTN(21, Conn.UniqueID);
                                DeleteBTN(22, Conn.UniqueID);
                            
                            #endregion

                                #region ' Restore some BTN '
                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                {
                                    if (Conn.InShop == true)
                                    {
                                        if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                        {
                                            InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                        }
                                    }
                                    if (Conn.InStore == true)
                                    {
                                        if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                        {
                                            InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                        }
                                    }
                                }
                                #endregion

                            MsgAll("^9 " + Conn.LastName + " is now ^1OFF-DUTY ^7as a Cadet!");
                            Conn.LastName = "";
                            Conn.IsCadet = false;
                        }
                        else
                        {
                            #region ' Remove Cop Panel '
                           
                                DeleteBTN(15, Conn.UniqueID);
                                DeleteBTN(16, Conn.UniqueID);
                                DeleteBTN(17, Conn.UniqueID);
                                DeleteBTN(18, Conn.UniqueID);
                                DeleteBTN(19, Conn.UniqueID);
                                DeleteBTN(20, Conn.UniqueID);
                                DeleteBTN(21, Conn.UniqueID);
                                DeleteBTN(22, Conn.UniqueID);
                            
                            #endregion

                            MsgAll("^9 " + Conn.LastName + " is now ^1OFF-DUTY ^7as a Cop!");
                            Conn.LastName = "";
                            Conn.IsCadet = false;
                        }
                    }
                    #endregion
                }
                #endregion

                #endregion

                #region ' tow truck check '
                if (CPR.PName.Contains(TowTruckTag))
                {
                    if (Conn.CanBeTowTruck == 1)
                    {
                        if (Conn.CurrentCar == "FBM")
                        {
                            MsgPly("^9 Cannot get duty whilst using FBM!", CPR.UCID);
                        }

                        else if (Conn.JobToHouse1 == false && Conn.JobToHouse2 == false && Conn.JobToHouse3 == false && Conn.JobToSchool == false)
                        {

                            if (CPR.Plate == "Tow")
                            {
                                if (Conn.IsTowTruck == false)
                                {
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

                                    MsgAll("^9 " + Conn.PlayerName + " is now ^2ON-DUTY ^7as Tow Truck!");

                                    if (Conn.CalledRequest == true)
                                    {
                                        Conn.CalledRequest = false;
                                    }

                                    Conn.LastName = Conn.PlayerName;
                                    Conn.IsTowTruck = true;
                                }
                            }
                            else
                            {
                                #region ' Remove Some '
                                if (Conn.IsTowTruck == true)
                                {
                                    if (Conn.InTowProgress == true)
                                    {
                                        MsgAll("^9 " + Conn.LastName + " stopped towing " + TowCon.PlayerName + "!");
                                        TowCon.IsBeingTowed = false;

                                        #region ' Restore some BTN '
                                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                        {
                                            if (Conn.InShop == true)
                                            {
                                                if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                                {
                                                    InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                    InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                                }
                                            }
                                            if (Conn.InStore == true)
                                            {
                                                if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                                {
                                                    InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                    InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                                }
                                            }
                                        }
                                        #endregion

                                        Conn.Towee = -1;
                                        Conn.InTowProgress = false;
                                        CautionSirenShutOff();
                                        MsgPly("^9 Your Platenumber must be in ' Police '!", CPR.UCID);
                                        MsgAll("^9 " + Conn.LastName + " is now ^1OFF-DUTY ^7as Tow Truck!");
                                        Conn.IsTowTruck = false;
                                        Conn.LastName = "";
                                    }
                                    else
                                    {
                                        #region ' Restore some BTN '
                                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                        {
                                            if (Conn.InShop == true)
                                            {
                                                if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                                {
                                                    InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                    InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                                }
                                            }
                                            if (Conn.InStore == true)
                                            {
                                                if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                                {
                                                    InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                    InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                                }
                                            }
                                        }
                                        #endregion

                                        MsgPly("^9 Your Platenumber must be in ' Police '!", CPR.UCID);
                                        MsgAll("^9 " + Conn.LastName + " is now ^1OFF-DUTY ^7as Tow Truck!");
                                        Conn.IsTowTruck = false;
                                        Conn.LastName = "";
                                    }
                                }
                                else
                                {
                                    MsgPly("^9 Your Platenumber must be in ' Tow '!", CPR.UCID);
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            MsgPly("^9 Cancel your current job first!", CPR.UCID);
                        }

                    }
                    else
                    {
                        MsgAll("^9 " + Conn.PlayerName + " is not a tow truck!");
                        MsgPly("^9 Please remove your tag!", CPR.UCID);
                        KickID(Conn.Username);
                    }
                }
                else
                {
                    #region ' Remove Some '
                    if (Conn.IsTowTruck == true)
                    {
                        if (Conn.InTowProgress == true)
                        {
                            MsgAll("^9 " + Conn.LastName + " stopped towing " + TowCon.PlayerName + "!");
                            TowCon.IsBeingTowed = false;

                            #region ' Restore some BTN '
                            if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                            {
                                if (Conn.InShop == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                                if (Conn.InStore == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                            }
                            #endregion

                            Conn.Towee = -1;
                            Conn.InTowProgress = false;
                            CautionSirenShutOff();
                            MsgAll("^9 " + Conn.LastName + " is now ^1OFF-DUTY ^7as Tow Truck!");
                            Conn.IsTowTruck = false;
                            Conn.LastName = "";
                        }
                        else
                        {
                            #region ' Restore some BTN '
                            if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                            {
                                if (Conn.InShop == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                                if (Conn.InStore == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                            }
                            #endregion

                            MsgAll("^9 " + Conn.LastName + " is now ^1OFF-DUTY ^7as Tow Truck!");
                            Conn.IsTowTruck = false;
                            Conn.LastName = "";
                        }
                    }
                    #endregion
                }
                #endregion

                // Update Connections[] list
                Conn.PlayerName = CPR.PName;
                Conn.Plate = CPR.Plate;
            }
            catch { }
        }

        // Car was taken over by an other player
        private void TOC_PlayerCarTakeOver(Packets.IS_TOC TOC)
        {
            try
            {
                var ConnOldUCID = Connections[GetConnIdx(TOC.OldUCID)];
                var ConnNewUCID = Connections[GetConnIdx(TOC.NewUCID)];

                if (RentingAllowed == true)
                {
                    #region ' Cars Not Allowed '
                    if (ConnNewUCID.CurrentCar == "FO8" || ConnNewUCID.CurrentCar == "BF1")
                    {
                        // To Rentee
                        Message("/spec " + ConnNewUCID.Username);
                        MsgPly("^9 This vehicle is not allowed to be rented", TOC.NewUCID);

                        // To Renter
                        MsgPly("^9 You are not allowed to rent that vehicle", TOC.OldUCID);
                        ConnOldUCID.BonusDistance = 0;
                        ConnOldUCID.TotalBonusDone = 0;
                        ConnOldUCID.AFKTick = 1;
                        ConnOldUCID.InGame = 0;
                    }
                    #endregion

                    #region ' Already Renting '
                    if (ConnOldUCID.Rented == 1)
                    {
                        // To Renter
                        Message("/spec " + ConnNewUCID.Username);
                        MsgPly("^9 " + ConnOldUCID.PlayerName + " is already renting a car", TOC.NewUCID);

                        // To Rentee
                        ConnOldUCID.BonusDistance = 0;
                        ConnOldUCID.TotalBonusDone = 0;
                        ConnOldUCID.AFKTick = 1;
                        ConnOldUCID.InGame = 0;
                        MsgPly("^9 You cannot rent more than ^71 ^3car at a time", TOC.OldUCID);
                    }
                    #endregion

                    #region ' New Renter '
                    if (ConnOldUCID.Renting == 0 && ConnOldUCID.Rented == 0 && ConnOldUCID.InGame == 1)
                    {
                        #region ' Exchange mode '
                        ConnNewUCID.BonusDistance = ConnOldUCID.BonusDistance;
                        ConnNewUCID.TotalBonusDone = ConnOldUCID.TotalBonusDone;
                        ConnOldUCID.BonusDistance = 0;
                        ConnOldUCID.TotalBonusDone = 0;
                        #endregion

                        #region ' Giving it. '
                        // Person Giving
                        ConnOldUCID.Renting = 1;
                        ConnOldUCID.Renter = ConnNewUCID.Username;
                        ConnOldUCID.AFKTick = 1;
                        ConnOldUCID.InGame = 0;

                        // Person Renting
                        ConnNewUCID.Rented = 1;
                        ConnNewUCID.Rentee = ConnOldUCID.Username;
                        ConnNewUCID.InGame = 1;
                        ConnNewUCID.AFKTick = 0;
                        #endregion

                        #region ' Transfer some '

                        #region ' Job To School '
                        if (ConnOldUCID.JobToSchool == true)
                        {
                            if (ConnOldUCID.JobFromHouse1 == true)
                            {
                                ConnNewUCID.JobFromHouse1 = ConnOldUCID.JobFromHouse1;
                                ConnOldUCID.JobFromHouse1 = false;
                            }
                            if (ConnOldUCID.JobFromHouse2 == true)
                            {
                                ConnNewUCID.JobFromHouse2 = ConnOldUCID.JobFromHouse2;
                                ConnOldUCID.JobFromHouse2 = false;
                            }
                            if (ConnOldUCID.JobFromHouse3 == true)
                            {
                                ConnNewUCID.JobFromHouse3 = ConnOldUCID.JobFromHouse3;
                                ConnOldUCID.JobFromHouse3 = false;
                            }
                            ConnNewUCID.JobToSchool = ConnOldUCID.JobToSchool;
                            ConnOldUCID.JobToSchool = false;
                        }
                        #endregion

                        #region ' Job From Shop '

                        if (ConnOldUCID.JobFromShop == true)
                        {
                            if (ConnOldUCID.JobToHouse1 == true)
                            {
                                ConnNewUCID.JobToHouse1 = ConnOldUCID.JobToHouse1;
                                ConnOldUCID.JobToHouse1 = false;
                            }

                            if (ConnOldUCID.JobToHouse2 == true)
                            {
                                ConnNewUCID.JobToHouse2 = ConnOldUCID.JobToHouse2;
                                ConnOldUCID.JobToHouse2 = false;
                            }

                            if (ConnOldUCID.JobToHouse3 == true)
                            {
                                ConnNewUCID.JobToHouse3 = ConnOldUCID.JobToHouse3;
                                ConnOldUCID.JobToHouse3 = false;
                            }

                            ConnNewUCID.JobFromShop = ConnOldUCID.JobFromShop;
                            ConnOldUCID.JobFromShop = false;
                        }

                        #endregion

                        #region ' Job From Store '

                        if (ConnOldUCID.JobFromStore == true)
                        {
                            if (ConnOldUCID.JobToHouse1 == true)
                            {
                                ConnNewUCID.JobToHouse1 = ConnOldUCID.JobToHouse1;
                                ConnOldUCID.JobToHouse1 = false;
                            }

                            if (ConnOldUCID.JobToHouse2 == true)
                            {
                                ConnNewUCID.JobToHouse2 = ConnOldUCID.JobToHouse2;
                                ConnOldUCID.JobToHouse2 = false;
                            }

                            if (ConnOldUCID.JobToHouse3 == true)
                            {
                                ConnNewUCID.JobToHouse3 = ConnOldUCID.JobToHouse3;
                                ConnOldUCID.JobToHouse3 = false;
                            }

                            ConnNewUCID.JobFromStore = ConnOldUCID.JobFromShop;
                            ConnOldUCID.JobFromStore = false;
                        }

                        #endregion

                        #endregion

                        #region ' Update Users '
                        FileInfo.SaveUser(
                            Connections[GetConnIdx(TOC.NewUCID)]);

                        FileInfo.SaveUser(
                            Connections[GetConnIdx(TOC.OldUCID)]);
                        #endregion

                        MsgAll("^9 Vehicle Rent");
                        MsgAll("^9 Owner : " + ConnOldUCID.PlayerName + " (" + ConnOldUCID.Username + ")");
                        MsgAll("^9 Renter : " + ConnNewUCID.PlayerName + " (" + ConnNewUCID.Username + ")");
                        MsgAll("^9 Vehicle : ^3" + ConnOldUCID.CurrentCar);
                    }
                    #endregion

                    #region ' Return Rent '
                    if (ConnOldUCID.Rented == 1)
                    {
                        if (ConnOldUCID.Rentee == ConnNewUCID.Username)
                        {
                            #region ' Not Chaining Rent '
                            // Renter

                            MsgPly("^9 You collected your rented vehicle back", TOC.NewUCID);
                            Connections[GetConnIdx(TOC.NewUCID)].BonusDistance = Connections[GetConnIdx(TOC.OldUCID)].BonusDistance;
                            Connections[GetConnIdx(TOC.NewUCID)].TotalBonusDone = Connections[GetConnIdx(TOC.OldUCID)].TotalBonusDone;
                            Connections[GetConnIdx(TOC.NewUCID)].Renting = 0;
                            Connections[GetConnIdx(TOC.NewUCID)].Renter = "";
                            Connections[GetConnIdx(TOC.NewUCID)].InGame = 1;
                            Connections[GetConnIdx(TOC.NewUCID)].AFKTick = 0;

                            // Owner

                            MsgPly("^9 You returned your vehicle to it's owner", TOC.OldUCID);
                            Connections[GetConnIdx(TOC.OldUCID)].BonusDistance = 0;
                            Connections[GetConnIdx(TOC.OldUCID)].TotalBonusDone = 0;
                            Connections[GetConnIdx(TOC.OldUCID)].Rented = 0;
                            Connections[GetConnIdx(TOC.OldUCID)].Rentee = "";
                            Connections[GetConnIdx(TOC.OldUCID)].AFKTick = 1;
                            Connections[GetConnIdx(TOC.OldUCID)].InGame = 0;

                            #region ' Transfer some '

                            #region ' Job To School '
                            if (ConnOldUCID.JobToSchool == true)
                            {
                                if (ConnOldUCID.JobFromHouse1 == true)
                                {
                                    ConnNewUCID.JobFromHouse1 = ConnOldUCID.JobFromHouse1;
                                    ConnOldUCID.JobFromHouse1 = false;
                                }
                                if (ConnOldUCID.JobFromHouse2 == true)
                                {
                                    ConnNewUCID.JobFromHouse2 = ConnOldUCID.JobFromHouse2;
                                    ConnOldUCID.JobFromHouse2 = false;
                                }
                                if (ConnOldUCID.JobFromHouse3 == true)
                                {
                                    ConnNewUCID.JobFromHouse3 = ConnOldUCID.JobFromHouse3;
                                    ConnOldUCID.JobFromHouse3 = false;
                                }
                                ConnNewUCID.JobToSchool = ConnOldUCID.JobToSchool;
                                ConnOldUCID.JobToSchool = false;
                            }
                            #endregion

                            #region ' Job From Shop '

                            if (ConnOldUCID.JobFromShop == true)
                            {
                                if (ConnOldUCID.JobToHouse1 == true)
                                {
                                    ConnNewUCID.JobToHouse1 = ConnOldUCID.JobToHouse1;
                                    ConnOldUCID.JobToHouse1 = false;
                                }

                                if (ConnOldUCID.JobToHouse2 == true)
                                {
                                    ConnNewUCID.JobToHouse2 = ConnOldUCID.JobToHouse2;
                                    ConnOldUCID.JobToHouse2 = false;
                                }

                                if (ConnOldUCID.JobToHouse3 == true)
                                {
                                    ConnNewUCID.JobToHouse3 = ConnOldUCID.JobToHouse3;
                                    ConnOldUCID.JobToHouse3 = false;
                                }

                                ConnNewUCID.JobFromShop = ConnOldUCID.JobFromShop;
                                ConnOldUCID.JobFromShop = false;
                            }

                            #endregion

                            #region ' Job From Store '

                            if (ConnOldUCID.JobFromStore == true)
                            {
                                if (ConnOldUCID.JobToHouse1 == true)
                                {
                                    ConnNewUCID.JobToHouse1 = ConnOldUCID.JobToHouse1;
                                    ConnOldUCID.JobToHouse1 = false;
                                }

                                if (ConnOldUCID.JobToHouse2 == true)
                                {
                                    ConnNewUCID.JobToHouse2 = ConnOldUCID.JobToHouse2;
                                    ConnOldUCID.JobToHouse2 = false;
                                }

                                if (ConnOldUCID.JobToHouse3 == true)
                                {
                                    ConnNewUCID.JobToHouse3 = ConnOldUCID.JobToHouse3;
                                    ConnOldUCID.JobToHouse3 = false;
                                }

                                ConnNewUCID.JobFromStore = ConnOldUCID.JobFromShop;
                                ConnOldUCID.JobFromStore = false;
                            }

                            #endregion

                            #endregion

                            #region ' Update Users '
                            FileInfo.SaveUser(
                                Connections[GetConnIdx(TOC.NewUCID)]);

                            FileInfo.SaveUser(
                                Connections[GetConnIdx(TOC.OldUCID)]);
                            #endregion

                            MsgAll("^9 Vehicle Rent Return");
                            MsgAll("^9 Owner : " + ConnNewUCID.PlayerName + " (" + ConnNewUCID.Username + ")");
                            MsgAll("^9 Renter : " + ConnOldUCID.PlayerName + " (" + ConnOldUCID.Username + ")");
                            MsgAll("^9 Vehicle : ^3" + ConnOldUCID.CurrentCar);
                            #endregion
                        }
                        else
                        {
                            #region ' Transfer some '

                            #region ' Job To School '
                            if (ConnOldUCID.JobToSchool == true)
                            {
                                if (ConnOldUCID.JobFromHouse1 == true)
                                {
                                    ConnOldUCID.JobFromHouse1 = false;
                                }
                                if (ConnOldUCID.JobFromHouse2 == true)
                                {
                                    ConnOldUCID.JobFromHouse2 = false;
                                }
                                if (ConnOldUCID.JobFromHouse3 == true)
                                {
                                    ConnOldUCID.JobFromHouse3 = false;
                                }
                                ConnOldUCID.JobToSchool = false;
                            }
                            #endregion

                            #region ' Job From Shop '

                            if (ConnOldUCID.JobFromShop == true)
                            {
                                if (ConnOldUCID.JobToHouse1 == true)
                                {
                                    ConnOldUCID.JobToHouse1 = false;
                                }

                                if (ConnOldUCID.JobToHouse2 == true)
                                {
                                    ConnOldUCID.JobToHouse2 = false;
                                }

                                if (ConnOldUCID.JobToHouse3 == true)
                                {
                                    ConnOldUCID.JobToHouse3 = false;
                                }

                                ConnOldUCID.JobFromShop = false;
                            }

                            #endregion

                            #region ' Job From Store '

                            if (ConnOldUCID.JobFromStore == true)
                            {
                                if (ConnOldUCID.JobToHouse1 == true)
                                {
                                    ConnOldUCID.JobToHouse1 = false;
                                }

                                if (ConnOldUCID.JobToHouse2 == true)
                                {
                                    ConnOldUCID.JobToHouse2 = false;
                                }

                                if (ConnOldUCID.JobToHouse3 == true)
                                {
                                    ConnOldUCID.JobToHouse3 = false;
                                }

                                ConnOldUCID.JobFromStore = false;
                            }

                            #endregion

                            #endregion

                            // To Renter
                            Message("/spec " + ConnNewUCID.Username);
                            MsgPly("^9 You are not allowed to chain rent vehicles", TOC.NewUCID);

                            // To Rentee
                            ConnOldUCID.BonusDistance = 0;
                            ConnOldUCID.TotalBonusDone = 0;
                            ConnOldUCID.AFKTick = 1;
                            ConnOldUCID.InGame = 0;
                            MsgPly("^9 You are not allowed to chain rent vehicles", TOC.OldUCID);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region ' Renting not Allowed '

                    if (ConnOldUCID.Rented == 1)
                    {
                        if (ConnOldUCID.Rentee == ConnNewUCID.Username)
                        {
                            #region ' Not Chaining Rent '
                            // Renter

                            MsgPly("^9 You collected your rented vehicle back", TOC.NewUCID);
                            Connections[GetConnIdx(TOC.NewUCID)].BonusDistance = Connections[GetConnIdx(TOC.OldUCID)].BonusDistance;
                            Connections[GetConnIdx(TOC.NewUCID)].TotalBonusDone = Connections[GetConnIdx(TOC.OldUCID)].TotalBonusDone;
                            Connections[GetConnIdx(TOC.NewUCID)].Renting = 0;
                            Connections[GetConnIdx(TOC.NewUCID)].Renter = "";
                            Connections[GetConnIdx(TOC.NewUCID)].InGame = 1;
                            Connections[GetConnIdx(TOC.NewUCID)].AFKTick = 0;

                            // Owner

                            MsgPly("^9 You returned your vehicle to it's owner", TOC.OldUCID);
                            Connections[GetConnIdx(TOC.OldUCID)].BonusDistance = 0;
                            Connections[GetConnIdx(TOC.OldUCID)].TotalBonusDone = 0;
                            Connections[GetConnIdx(TOC.OldUCID)].Rented = 0;
                            Connections[GetConnIdx(TOC.OldUCID)].Rentee = "";
                            Connections[GetConnIdx(TOC.OldUCID)].AFKTick = 1;
                            Connections[GetConnIdx(TOC.OldUCID)].InGame = 0;

                            #region ' Transfer some '

                            #region ' Job To School '
                            if (ConnOldUCID.JobToSchool == true)
                            {
                                if (ConnOldUCID.JobFromHouse1 == true)
                                {
                                    ConnNewUCID.JobFromHouse1 = ConnOldUCID.JobFromHouse1;
                                    ConnOldUCID.JobFromHouse1 = false;
                                }
                                if (ConnOldUCID.JobFromHouse2 == true)
                                {
                                    ConnNewUCID.JobFromHouse2 = ConnOldUCID.JobFromHouse2;
                                    ConnOldUCID.JobFromHouse2 = false;
                                }
                                if (ConnOldUCID.JobFromHouse3 == true)
                                {
                                    ConnNewUCID.JobFromHouse3 = ConnOldUCID.JobFromHouse3;
                                    ConnOldUCID.JobFromHouse3 = false;
                                }
                                ConnNewUCID.JobToSchool = ConnOldUCID.JobToSchool;
                                ConnOldUCID.JobToSchool = false;
                            }
                            #endregion

                            #region ' Job From Shop '

                            if (ConnOldUCID.JobFromShop == true)
                            {
                                if (ConnOldUCID.JobToHouse1 == true)
                                {
                                    ConnNewUCID.JobToHouse1 = ConnOldUCID.JobToHouse1;
                                    ConnOldUCID.JobToHouse1 = false;
                                }

                                if (ConnOldUCID.JobToHouse2 == true)
                                {
                                    ConnNewUCID.JobToHouse2 = ConnOldUCID.JobToHouse2;
                                    ConnOldUCID.JobToHouse2 = false;
                                }

                                if (ConnOldUCID.JobToHouse3 == true)
                                {
                                    ConnNewUCID.JobToHouse3 = ConnOldUCID.JobToHouse3;
                                    ConnOldUCID.JobToHouse3 = false;
                                }

                                ConnNewUCID.JobFromShop = ConnOldUCID.JobFromShop;
                                ConnOldUCID.JobFromShop = false;
                            }

                            #endregion

                            #region ' Job From Store '

                            if (ConnOldUCID.JobFromStore == true)
                            {
                                if (ConnOldUCID.JobToHouse1 == true)
                                {
                                    ConnNewUCID.JobToHouse1 = ConnOldUCID.JobToHouse1;
                                    ConnOldUCID.JobToHouse1 = false;
                                }

                                if (ConnOldUCID.JobToHouse2 == true)
                                {
                                    ConnNewUCID.JobToHouse2 = ConnOldUCID.JobToHouse2;
                                    ConnOldUCID.JobToHouse2 = false;
                                }

                                if (ConnOldUCID.JobToHouse3 == true)
                                {
                                    ConnNewUCID.JobToHouse3 = ConnOldUCID.JobToHouse3;
                                    ConnOldUCID.JobToHouse3 = false;
                                }

                                ConnNewUCID.JobFromStore = ConnOldUCID.JobFromShop;
                                ConnOldUCID.JobFromStore = false;
                            }

                            #endregion

                            #endregion

                            #region ' Update Users '
                            FileInfo.SaveUser(
                                Connections[GetConnIdx(TOC.NewUCID)]);

                            FileInfo.SaveUser(
                                Connections[GetConnIdx(TOC.OldUCID)]);
                            #endregion

                            MsgAll("^9 Vehicle Rent Return");
                            MsgAll("^9 Owner : " + ConnNewUCID.PlayerName + " (" + ConnNewUCID.Username + ")");
                            MsgAll("^9 Renter : " + ConnOldUCID.PlayerName + " (" + ConnOldUCID.Username + ")");
                            MsgAll("^9 Vehicle : ^3" + ConnOldUCID.CurrentCar);
                            #endregion
                        }
                        else
                        {
                            #region ' chaining rent '
                            #region ' Transfer some '

                            #region ' Job To School '
                            if (ConnOldUCID.JobToSchool == true)
                            {
                                if (ConnOldUCID.JobFromHouse1 == true)
                                {
                                    ConnOldUCID.JobFromHouse1 = false;
                                }
                                if (ConnOldUCID.JobFromHouse2 == true)
                                {
                                    ConnOldUCID.JobFromHouse2 = false;
                                }
                                if (ConnOldUCID.JobFromHouse3 == true)
                                {
                                    ConnOldUCID.JobFromHouse3 = false;
                                }
                                ConnOldUCID.JobToSchool = false;
                            }
                            #endregion

                            #region ' Job From Shop '

                            if (ConnOldUCID.JobFromShop == true)
                            {
                                if (ConnOldUCID.JobToHouse1 == true)
                                {
                                    ConnOldUCID.JobToHouse1 = false;
                                }

                                if (ConnOldUCID.JobToHouse2 == true)
                                {
                                    ConnOldUCID.JobToHouse2 = false;
                                }

                                if (ConnOldUCID.JobToHouse3 == true)
                                {
                                    ConnOldUCID.JobToHouse3 = false;
                                }

                                ConnOldUCID.JobFromShop = false;
                            }

                            #endregion

                            #region ' Job From Store '

                            if (ConnOldUCID.JobFromStore == true)
                            {
                                if (ConnOldUCID.JobToHouse1 == true)
                                {
                                    ConnOldUCID.JobToHouse1 = false;
                                }

                                if (ConnOldUCID.JobToHouse2 == true)
                                {
                                    ConnOldUCID.JobToHouse2 = false;
                                }

                                if (ConnOldUCID.JobToHouse3 == true)
                                {
                                    ConnOldUCID.JobToHouse3 = false;
                                }

                                ConnOldUCID.JobFromStore = false;
                            }

                            #endregion

                            #endregion

                            // To Renter
                            Message("/spec " + ConnNewUCID.Username);
                            MsgPly("^9 You are not allowed to chain rent vehicles", TOC.NewUCID);

                            // To Rentee
                            ConnOldUCID.BonusDistance = 0;
                            ConnOldUCID.TotalBonusDone = 0;
                            ConnOldUCID.AFKTick = 1;
                            ConnOldUCID.InGame = 0;
                            if (ConnOldUCID.Rented == 1)
                            {
                                bool Found = false;

                                #region ' Online '
                                foreach (clsConnection Conn in Connections)
                                {
                                    if (Conn.Username == ConnOldUCID.Rentee)
                                    {
                                        Found = true;
                                        Conn.Renting = 0;
                                        Conn.Renter = "x";
                                        MsgPly("^9 Your rental was returned to you", Conn.UniqueID);
                                    }
                                }
                                #endregion

                                #region ' Offline '
                                if (Found == false)
                                {
                                    #region ' Objects '

                                    long Cash = 0;
                                    long BBal = 0;
                                    string Cars = "";
                                    long Distance = 0;
                                    byte Health = 0;
                                    long Gold = 0;
                                    byte Goods2 = 0;
                                    byte Goods1 = 0;
                                    int Raffle = 0;
                                    int Lotto = 0;
                                    int JobsDone = 0;

                                    byte Officer = 0;
                                    byte Cadet = 0;
                                    byte TowTruck = 0;
                                    byte IsMember = 0;

                                    byte Intrfc1 = 0;
                                    byte Intrfc2 = 0;
                                    byte Speedo = 0;
                                    byte Odometer = 0;
                                    byte Counter = 0;
                                    byte Panel = 0;

                                    byte Renting = 0;
                                    byte Rented = 0;
                                    string Renter = "";
                                    string Renterr = "";
                                    string Rentee = "";

                                    string PlayerInfo = "";
                                    #endregion

                                    #region ' Read SR Info '
                                    StreamReader Sr = new StreamReader(Database + "\\" + ConnOldUCID.Rentee + ".txt");
                                    string line = null;

                                    while ((line = Sr.ReadLine()) != null)
                                    {
                                        #region ' Player Stats '
                                        if (line.Substring(0, 4) == "Cash")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Cash = long.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 4) == "BBal")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            BBal = long.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 4) == "Gold")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Gold = long.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 4) == "Cars")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Cars = Msg1[1].Trim();
                                        }
                                        if (line.Substring(0, 8) == "Distance")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Distance = long.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 6) == "Health")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Health = byte.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 8) == "JobsDone")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            JobsDone = int.Parse(Msg1[1].Trim());
                                        }

                                        if (line.Substring(0, 6) == "Goods1")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Goods1 = byte.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 6) == "Goods2")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Goods2 = byte.Parse(Msg1[1].Trim());
                                        }

                                        // Timers
                                        if (line.Substring(0, 6) == "Lotto")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Lotto = Int32.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 6) == "Raffle")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Raffle = Int32.Parse(Msg1[1].Trim());
                                        }
                                        #endregion

                                        #region ' Player Status '
                                        // Service Status
                                        if (line.Substring(0, 7) == "Officer")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Officer = byte.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 6) == "Member")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            IsMember = byte.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 7) == "TowTruck")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            TowTruck = byte.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 5) == "Cadet")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Cadet = byte.Parse(Msg1[1].Trim());
                                        }
                                        #endregion

                                        #region ' User Settings '
                                        // Player Settings
                                        if (line.Substring(0, 7) == "Intrfc1")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Intrfc1 = byte.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 7) == "Intrfc2")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Intrfc2 = byte.Parse(Msg1[1].Trim());
                                        }

                                        if (line.Substring(0, 6) == "Speedo")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Speedo = byte.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 8) == "Odometer")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Odometer = byte.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 7) == "Counter")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Counter = byte.Parse(Msg1[1].Trim());
                                        }

                                        if (line.Substring(0, 5) == "Panel")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Panel = byte.Parse(Msg1[1].Trim());
                                        }
                                        #endregion

                                        #region ' Renting '
                                        // Player Info
                                        if (line.Substring(0, 7) == "Renting")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Renting = byte.Parse(Msg1[1].Trim());
                                        }
                                        
                                        if (line.Substring(0, 6) == "Rented")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Rented = byte.Parse(Msg1[1].Trim());
                                        }

                                        if (line.Substring(0, 6) == "Rentee")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Rentee = Msg1[1].Trim();
                                        }

                                        if (line.Substring(0, 6) == "Renter")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Renter = Msg1[1].Trim();
                                        }

                                        if (line.Substring(0, 7) == "Renterr")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Renterr = Msg1[1].Trim();
                                        }

                                        #endregion

                                        #region ' Player Name LOL '
                                        if (line.Substring(0, 7) == "RegInfo")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            PlayerInfo = Msg1[1].Trim();
                                        }
                                        #endregion
                                    }
                                    Sr.Close();
                                    #endregion

                                    #region ' Remove Renting '

                                    Renting = 0;
                                    Renter = "x";

                                    #endregion

                                    #region ' Special PlayerName Colors Remove '

                                    string PlayerName = PlayerInfo;
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

                                    MsgAll("^9 " + ConnOldUCID.PlayerName + " his/her rentals returned to " + PlayerName);

                                    #region ' Save User '

                                    StreamWriter Sw = new StreamWriter(Database + "\\" + ConnOldUCID.Rentee + ".txt");
                                    Sw.WriteLine("Cash = " + Cash);
                                    Sw.WriteLine("BBal = " + BBal);
                                    Sw.WriteLine("Gold = " + Gold);
                                    Sw.WriteLine("Cars = " + Cars);
                                    Sw.WriteLine("Distance = " + Distance);
                                    Sw.WriteLine("Health = " + Health);
                                    Sw.WriteLine("JobsDone = " + JobsDone);

                                    Sw.WriteLine("Goods1 = " + Goods1);
                                    Sw.WriteLine("Goods2 = " + Goods2);

                                    Sw.WriteLine("Member = " + IsMember);
                                    Sw.WriteLine("Officer = " + Officer);
                                    Sw.WriteLine("Cadet = " + Cadet);
                                    Sw.WriteLine("TowTruck = " + TowTruck);

                                    Sw.WriteLine("Raffle = " + Raffle);
                                    Sw.WriteLine("Lotto = " + Lotto);

                                    Sw.WriteLine("Intrfc1 = " + Intrfc1);
                                    Sw.WriteLine("Intrfc2 = " + Intrfc2);
                                    Sw.WriteLine("Speedo = " + Speedo);
                                    Sw.WriteLine("Odometer = " + Odometer);
                                    Sw.WriteLine("Counter = " + Counter);

                                    Sw.WriteLine("Renting = " + Renting);
                                    Sw.WriteLine("Rented = " + Rented);
                                    Sw.WriteLine("Renter = " + Renter);
                                    Sw.WriteLine("Renterr = " + Renterr);
                                    Sw.WriteLine("Rentee = " + Rentee);

                                    Sw.WriteLine("RegInfo = " + PlayerInfo);

                                    Sw.WriteLine("//// " + System.DateTime.Now);
                                    Sw.Flush();
                                    Sw.Close();

                                    #endregion
                                }
                                #endregion

                                Connections[GetConnIdx(TOC.OldUCID)].Rented = 0;
                                Connections[GetConnIdx(TOC.OldUCID)].Rentee = "x";
                            }
                            MsgPly("^9 You are not allowed to chain rent vehicles", TOC.OldUCID);
                            #endregion
                        }
                    }
                    else
                    {
                        #region ' Not Returning Rent '
                        // To Rentee
                        Message("/spec " + ConnNewUCID.Username);
                        MsgPly("^9 Renting is Currently Disabled", TOC.NewUCID);

                        // To Renter
                        MsgPly("^9 Renting is Currently Disabled", TOC.OldUCID);
                        ConnOldUCID.BonusDistance = 0;
                        ConnOldUCID.TotalBonusDone = 0;
                        ConnOldUCID.AFKTick = 1;
                        ConnOldUCID.InGame = 0;

                        #region ' Transfer some '

                        #region ' Job To School '
                        if (ConnOldUCID.JobToSchool == true)
                        {
                            if (ConnOldUCID.JobFromHouse1 == true)
                            {
                                ConnOldUCID.JobFromHouse1 = false;
                            }
                            if (ConnOldUCID.JobFromHouse2 == true)
                            {
                                ConnOldUCID.JobFromHouse2 = false;
                            }
                            if (ConnOldUCID.JobFromHouse3 == true)
                            {
                                ConnOldUCID.JobFromHouse3 = false;
                            }
                            ConnOldUCID.JobToSchool = false;
                        }
                        #endregion

                        #region ' Job From Shop '

                        if (ConnOldUCID.JobFromShop == true)
                        {
                            if (ConnOldUCID.JobToHouse1 == true)
                            {
                                ConnOldUCID.JobToHouse1 = false;
                            }

                            if (ConnOldUCID.JobToHouse2 == true)
                            {
                                ConnOldUCID.JobToHouse2 = false;
                            }

                            if (ConnOldUCID.JobToHouse3 == true)
                            {
                                ConnOldUCID.JobToHouse3 = false;
                            }

                            ConnOldUCID.JobFromShop = false;
                        }

                        #endregion

                        #region ' Job From Store '

                        if (ConnOldUCID.JobFromStore == true)
                        {
                            if (ConnOldUCID.JobToHouse1 == true)
                            {
                                ConnOldUCID.JobToHouse1 = false;
                            }

                            if (ConnOldUCID.JobToHouse2 == true)
                            {
                                ConnOldUCID.JobToHouse2 = false;
                            }

                            if (ConnOldUCID.JobToHouse3 == true)
                            {
                                ConnOldUCID.JobToHouse3 = false;
                            }

                            ConnOldUCID.JobFromStore = false;
                        }

                        #endregion

                        #endregion
                        #endregion
                    }

                    #endregion
                }

                // New
                ConnNewUCID.PlayerID = TOC.PLID;
                ConnNewUCID.CurrentCar = ConnOldUCID.CurrentCar;

                // Old 
                ConnOldUCID.PlayerID = 0;
                ConnOldUCID.CompCar = new Packets.CompCar();
            }
            catch { }
        }

        // A player leaves the race (spectate)
        private void PLL_PlayerLeavesRace(Packets.IS_PLL PLL)
        {
            try
            {
                #region ' UniqueID Loader '
                int IDX = -1;
                for (int i = 0; i < Connections.Count; i++)
                {
                    if (Connections[i].PlayerID == PLL.PLID)
                    {
                        IDX = i;
                        break;
                    }
                }
                if (IDX == -1)
                    return;
                clsConnection Conn = Connections[IDX];
                var ChaseCon = Connections[GetConnIdx(Connections[IDX].Chasee)];
                var TowCon = Connections[GetConnIdx(Connections[IDX].Towee)];
                #endregion

                #region ' In Game Necessities '
                if (Conn.InGame == 1)
                {

                    

                    #region ' On Screen PitLane Clear '
                    if (Conn.LeavesPitLane == 1)
                    {
                        Conn.LeavesPitLane = 0;
                    }
                    if (Conn.OnScreenExit > 0)
                    {
                        DeleteBTN(10, Conn.UniqueID);
                        Conn.OnScreenExit = 0;
                    }
                    #endregion

                    #region ' Bonus Done Region '
                    if (Conn.TotalBonusDone == 0)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 400 + "%");
                    }
                    else if (Conn.TotalBonusDone == 1)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 800 + "%");
                    }
                    else if (Conn.TotalBonusDone == 2)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 3)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 1200 + "%");
                    }
                    else if (Conn.TotalBonusDone == 4)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 1600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 5)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 2000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 6)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 2600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 7)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 3200 + "%");
                    }
                    else if (Conn.TotalBonusDone == 8)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 4000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 9)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 5400 + "%");
                    }
                    else if (Conn.TotalBonusDone == 10)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 6600 + "%");
                    }
                    #endregion

                    #region ' OnScreen Sign '

                    if (Conn.MapSignActivated == true)
                    {
                        if (Conn.MapSigns > 0)
                        {
                            DeleteBTN(10, Conn.UniqueID);
                            Conn.MapSigns = 0;
                        }
                    }

                    #endregion

                    #region ' OnScreen Ahead '

                    if (Conn.StreetSign > 0)
                    {
                        DeleteBTN(11, Conn.UniqueID);
                        DeleteBTN(12, Conn.UniqueID);
                        DeleteBTN(13, Conn.UniqueID);
                        Conn.StreetSign = 0;
                    }

                    #endregion

                    #region ' Remove Cop Panel '
                    if (Conn.IsOfficer == true || Conn.IsCadet == true)
                    {
                        #region ' Remove Cop Panel '

                        DeleteBTN(15, Conn.UniqueID);
                        DeleteBTN(16, Conn.UniqueID);
                        DeleteBTN(17, Conn.UniqueID);
                        DeleteBTN(18, Conn.UniqueID);
                        DeleteBTN(19, Conn.UniqueID);
                        DeleteBTN(20, Conn.UniqueID);
                        DeleteBTN(21, Conn.UniqueID);
                        DeleteBTN(22, Conn.UniqueID);

                        #endregion
                    }
                    #endregion

                    #region ' Close BTN '
                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                    {
                        if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true || Conn.InSchool == true || Conn.InShop == true || Conn.InStore == true || Conn.InBank == true)
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
                        }
                        Conn.DisplaysOpen = false;
                    }
                    #endregion

                    #region ' Close Location '

                    if (Conn.InBank == true)
                    {
                        Conn.InBank = false;
                    }
                    if (Conn.InHouse1 == true)
                    {
                        Conn.InHouse1 = false;
                    }
                    if (Conn.InHouse2 == true)
                    {
                        Conn.InHouse2 = false;
                    }
                    if (Conn.InHouse3 == true)
                    {
                        Conn.InHouse3 = false;
                    }
                    if (Conn.InSchool == true)
                    {
                        Conn.InSchool = false;
                    }

                    if (Conn.InShop == true)
                    {
                        Conn.InShop = false;
                    }
                    if (Conn.InStore == true)
                    {
                        Conn.InStore = false;
                    }
                    #endregion

                    Conn.TotalBonusDone = 0;
                    Conn.BonusDistance = 0;
                    Conn.InGame = 0;
                }
                #endregion
               
                #region ' Cop System '

                if (Conn.TrapSetted == true)
                {
                    MsgPly("^9 Speed Trap Removed", Conn.UniqueID);
                    Conn.TrapY = 0;
                    Conn.TrapX = 0;
                    Conn.TrapSpeed = 0;
                    Conn.TrapSetted = false;
                }

                if (Conn.IsSuspect == true)
                {
                    MsgAll("^9 " + Conn.PlayerName + " was fined ^1$5000");
                    MsgAll("  ^7For specting on track whilst being chased!");
                    Conn.Cash -= 5000;

                    #region ' In Connection List '
                    foreach (clsConnection i in Connections)
                    {
                        if (i.Chasee == Conn.UniqueID)
                        {
                            if (i.JoinedChase == true)
                            {
                                i.JoinedChase = false;
                            }
                            i.BustedTimer = 0;
                            i.Busted = false;
                            i.AutoBumpTimer = 0;
                            i.BumpButton = 0;
                            i.ChaseCondition = 0;
                            i.InChaseProgress = false;
                            i.Chasee = -1;
                        }
                    }
                    #endregion

                    AddChaseLimit -= 1;
                    Conn.PullOvrMsg = 0;
                    Conn.ChaseCondition = 0;
                    Conn.CopInChase = 0;
                    Conn.IsSuspect = false;
                    CopSirenShutOff();
                }

                if (Conn.InFineMenu == true)
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

                if (Conn.IsBeingBusted == true)
                {
                    MsgAll("^9 " + Conn.PlayerName + " was fined ^1$5000");
                    MsgAll("  ^7For specting on track whilst being busted!");
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
                                MsgAll("^9 " + i.PlayerName + " was rewarded for ^2$" + (Convert.ToInt16(5000 * 0.2)));
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

                    Conn.PullOvrMsg = 0;
                    Conn.ChaseCondition = 0;
                    Conn.AcceptTicket = 0;
                    Conn.TicketRefuse = 0;
                    Conn.CopInChase = 0;
                    Conn.IsBeingBusted = false;
                }

                if (Conn.AcceptTicket == 2)
                {
                    #region ' Connection List '
                    foreach (clsConnection i in Connections)
                    {
                        if (i.Chasee == Conn.UniqueID)
                        {
                            if (i.InFineMenu == true)
                            {
                                i.InFineMenu = false;
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

                    Conn.AcceptTicket = 0;
                    Conn.IsBeingBusted = false;
                    Conn.CopInChase = 0;
                    Conn.TicketRefuse = 0;

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
                }

                if (Conn.InChaseProgress == true)
                {
                    if (ChaseCon.CopInChase > 1)
                    {
                        if (Conn.JoinedChase == true)
                        {
                            Conn.JoinedChase = false;
                        }
                        Conn.ChaseCondition = 0;
                        Conn.Busted = false;
                        Conn.BustedTimer = 0;
                        Conn.BumpButton = 0;
                        Conn.Chasee = -1;
                        ChaseCon.CopInChase -= 1;

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

                        MsgAll("^9 " + Conn.PlayerName + " sighting lost " + ChaseCon.PlayerName + "!");
                        MsgAll("   ^7 Total Cops In Chase: " + ChaseCon.CopInChase);
                    }
                    else if (ChaseCon.CopInChase == 1)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " lost " + ChaseCon.PlayerName + "!");
                        MsgAll("   ^7Suspect Runs away from being chased!");
                        AddChaseLimit -= 1;
                        Conn.AutoBumpTimer = 0;
                        Conn.BumpButton = 0;
                        Conn.BustedTimer = 0;
                        Conn.Chasee = -1;
                        Conn.Busted = false;
                        ChaseCon.PullOvrMsg = 0;
                        ChaseCon.ChaseCondition = 0;
                        ChaseCon.CopInChase = 0;
                        ChaseCon.IsSuspect = false;
                        Conn.ChaseCondition = 0;
                        CopSirenShutOff();
                    }

                    Conn.InChaseProgress = false;
                }

                #endregion

                #region ' Return Rent '
                if (Conn.Rented == 1)
                {
                    bool Found = false;

                    #region ' Online '
                    foreach (clsConnection C in Connections)
                    {
                        if (C.Username == Conn.Rentee)
                        {
                            Found = true;
                            C.Renting = 0;
                            C.Renter = "0";
                            MsgAll("^9 " + Conn.PlayerName + " their rentals returned to " + C.PlayerName);
                        }
                    }
                    #endregion

                    #region ' Offline '
                    if (Found == false)
                    {
                        #region ' Objects '

                        long Cash = FileInfo.GetUserCash(Conn.Rentee);
                        long BBal = FileInfo.GetUserBank(Conn.Rentee);
                        string Cars = FileInfo.GetUserCars(Conn.Rentee);
                        long Gold = FileInfo.GetUserGold(Conn.Rentee);

                        long TotalDistance = FileInfo.GetUserDistance(Conn.Rentee);
                        byte TotalHealth = FileInfo.GetUserHealth(Conn.Rentee);
                        int TotalJobsDone = FileInfo.GetUserJobsDone(Conn.Rentee);

                        byte Electronics = FileInfo.GetUserElectronics(Conn.Rentee);
                        byte Furniture = FileInfo.GetUserFurniture(Conn.Rentee);

                        int LastRaffle = FileInfo.GetUserLastRaffle(Conn.Rentee);
                        int LastLotto = FileInfo.GetUserLastLotto(Conn.Rentee);

                        byte CanBeOfficer = FileInfo.CanBeOfficer(Conn.Rentee);
                        byte CanBeCadet = FileInfo.CanBeCadet(Conn.Rentee);
                        byte CanBeTowTruck = FileInfo.CanBeTowTruck(Conn.Rentee);
                        byte IsModerator = FileInfo.IsMember(Conn.Rentee);

                        byte Interface1 = FileInfo.GetInterface(Conn.Rentee);
                        byte Interface2 = FileInfo.GetInterface2(Conn.Rentee);
                        byte Speedo = FileInfo.GetSpeedo(Conn.Rentee);
                        byte Odometer = FileInfo.GetOdometer(Conn.Rentee);
                        byte Counter = FileInfo.GetCounter(Conn.Rentee);
                        byte Panel = FileInfo.GetCopPanel(Conn.Rentee);

                        byte Renting = FileInfo.GetUserRenting(Conn.Rentee);
                        byte Rented = FileInfo.GetUserRented(Conn.Rentee);
                        string Renter = FileInfo.GetUserRenter(Conn.Rentee);
                        string Renterr = FileInfo.GetUserRenterr(Conn.Rentee);
                        string Rentee = FileInfo.GetUserRentee(Conn.Rentee);

                        string PlayerName = FileInfo.GetUserPlayerName(Conn.Rentee);
                        #endregion

                        #region ' Remove Renting '

                        Renting = 0;
                        Renter = "0";

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

                        MsgAll("^9 " + Conn.PlayerName + " their rentals returned to " + PlayerName);

                        #region ' Save User '

                        FileInfo.SaveOfflineUser(Conn.Rentee,
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
                    #endregion

                    Conn.Rentee = "0";
                    Conn.Rented = 0;
                }
                #endregion

                #region ' Tow System '

                if (Conn.InTowProgress == true)
                {
                    if (TowCon.IsBeingTowed == true)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " stopped towing " + TowCon.PlayerName + "!");
                        TowCon.IsBeingTowed = false;
                    }
                    Conn.Towee = -1;
                    Conn.InTowProgress = false;
                    CautionSirenShutOff();
                }

                if (Conn.IsBeingTowed == true)
                {
                    MsgAll("^9 " + Conn.PlayerName +" spected whilst being Towed!");
                    foreach (clsConnection o in Connections)
                    {
                        if (o.Towee == Conn.UniqueID)
                        {
                            o.InTowProgress = false;
                            o.Towee = -1;
                        }
                    }
                    Conn.IsBeingTowed = false;
                    CautionSirenShutOff();
                }

                #endregion

                Conn.TripMeter = 0;
                Conn.Location = "Spectators";
                Conn.LastSeen = "Spectators";
                Conn.LocationBox = "^7Spectators";
                Conn.SpeedBox = "";
                // Update Players[] list
                Conn.CompCar = new Packets.CompCar();
            }
            catch { }
        }

        // A player goes to the garage (setup screen).
        private void PLP_PlayerGoesToGarage(Packets.IS_PLP PLP)
        {
            try
            {
                #region ' UniqueID Loader '
                int IDX = -1;
                for (int i = 0; i < Connections.Count; i++)
                {
                    if (Connections[i].PlayerID == PLP.PLID)
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

                #region ' In Game Neccesities '
                if (Conn.InGame == 1)
                {
                    #region ' Bonus Done Region '
                    if (Conn.TotalBonusDone == 0)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 400 + "%");
                    }
                    else if (Conn.TotalBonusDone == 1)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 800 + "%");
                    }
                    else if (Conn.TotalBonusDone == 2)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 3)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 1200 + "%");
                    }
                    else if (Conn.TotalBonusDone == 4)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 1600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 5)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 2000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 6)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 2600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 7)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 3200 + "%");
                    }
                    else if (Conn.TotalBonusDone == 8)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 4000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 9)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 5400 + "%");
                    }
                    else if (Conn.TotalBonusDone == 10)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " ends their trip at ^3" + Conn.BonusDistance / 6600 + "%");
                    }
                    #endregion

                    #region ' OnScreen Spectators '
                    if (Conn.WaitIntrfc == 0)
                    {
                        if (Conn.Interface == 0)
                        {
                            DeleteBTN(8, Conn.UniqueID);
                            InSim.Send_BTN_CreateButton("^7Spectators", Flags.ButtonStyles.ISB_DARK, 5, 25, 86, 172, 9, Conn.UniqueID, 2, false);
                        }
                    }
                    #endregion

                    #region ' On Screen PitLane Clear '
                    if (Conn.LeavesPitLane == 1)
                    {
                        Conn.LeavesPitLane = 0;
                    }
                    if (Conn.OnScreenExit > 0)
                    {
                        DeleteBTN(10, Conn.UniqueID);
                        Conn.OnScreenExit = 0;
                    }

                    #endregion

                    #region ' OnScreen Ahead '

                    if (Conn.StreetSign > 0)
                    {
                        DeleteBTN(11, Conn.UniqueID);
                        DeleteBTN(12, Conn.UniqueID);
                        DeleteBTN(13, Conn.UniqueID);
                        Conn.StreetSign = 0;
                    }

                    #endregion

                    #region ' OnScreen Sign '

                    if (Conn.MapSignActivated == true)
                    {
                        if (Conn.MapSigns > 0)
                        {
                            DeleteBTN(10, Conn.UniqueID);
                            Conn.MapSigns = 0;
                        }
                    }

                    #endregion

                    #region ' Remove Cop Panel '
                    if (Conn.IsOfficer == true || Conn.IsCadet == true)
                    {
                        #region ' Remove Cop Panel '

                        DeleteBTN(15, Conn.UniqueID);
                        DeleteBTN(16, Conn.UniqueID);
                        DeleteBTN(17, Conn.UniqueID);
                        DeleteBTN(18, Conn.UniqueID);
                        DeleteBTN(19, Conn.UniqueID);
                        DeleteBTN(20, Conn.UniqueID);
                        DeleteBTN(21, Conn.UniqueID);
                        DeleteBTN(22, Conn.UniqueID);

                        #endregion
                    }
                    #endregion

                    #region ' Close BTN '
                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                    {
                        if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true || Conn.InSchool == true || Conn.InShop == true || Conn.InStore == true || Conn.InBank == true)
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
                        }
                        Conn.DisplaysOpen = false;
                    }
                    #endregion

                    #region ' Close Location '

                    if (Conn.InBank == true)
                    {
                        Conn.InBank = false;
                    }
                    if (Conn.InHouse1 == true)
                    {
                        Conn.InHouse1 = false;
                    }
                    if (Conn.InHouse2 == true)
                    {
                        Conn.InHouse2 = false;
                    }
                    if (Conn.InHouse3 == true)
                    {
                        Conn.InHouse3 = false;
                    }
                    if (Conn.InSchool == true)
                    {
                        Conn.InSchool = false;
                    }

                    if (Conn.InShop == true)
                    {
                        Conn.InShop = false;
                    }
                    if (Conn.InStore == true)
                    {
                        Conn.InStore = false;
                    }
                    #endregion

                    Conn.TotalBonusDone = 0;
                    Conn.BonusDistance = 0;
                    Conn.InGame = 0;
                }
                #endregion

                #region ' Cop System '

                if (Conn.TrapSetted == true)
                {
                    MsgPly("^9 Speed Trap Removed", Conn.UniqueID);
                    Conn.TrapY = 0;
                    Conn.TrapX = 0;
                    Conn.TrapSpeed = 0;
                    Conn.TrapSetted = false;
                }

                if (Conn.IsSuspect == true)
                {
                    MsgAll("^9 " + Conn.PlayerName + " was fined ^1$5000");
                    MsgAll("  ^7For specting on track whilst being chased!");
                    Conn.Cash -= 5000;

                    #region ' In Connection List '
                    foreach (clsConnection i in Connections)
                    {
                        if (i.Chasee == Conn.UniqueID)
                        {
                            if (i.JoinedChase == true)
                            {
                                i.JoinedChase = false;
                            }
                            i.BustedTimer = 0;
                            i.Busted = false;
                            i.AutoBumpTimer = 0;
                            i.BumpButton = 0;
                            i.ChaseCondition = 0;
                            i.InChaseProgress = false;
                            i.Chasee = -1;
                        }
                    }
                    #endregion

                    AddChaseLimit -= 1;
                    Conn.PullOvrMsg = 0;
                    Conn.ChaseCondition = 0;
                    Conn.CopInChase = 0;
                    Conn.IsSuspect = false;
                    CopSirenShutOff();
                }

                if (Conn.InFineMenu == true)
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

                if (Conn.IsBeingBusted == true)
                {
                    MsgAll("^9 " + Conn.PlayerName + " was fined ^1$5000");
                    MsgAll("  ^7For specting on track whilst being busted!");
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
                                MsgAll("^9 " + i.PlayerName + " was rewarded for ^2$" + (Convert.ToInt16(5000 * 0.2)));
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

                    Conn.PullOvrMsg = 0;
                    Conn.ChaseCondition = 0;
                    Conn.AcceptTicket = 0;
                    Conn.TicketRefuse = 0;
                    Conn.CopInChase = 0;
                    Conn.IsBeingBusted = false;
                }

                if (Conn.AcceptTicket == 2)
                {
                    #region ' Connection List '
                    foreach (clsConnection i in Connections)
                    {
                        if (i.Chasee == Conn.UniqueID)
                        {
                            if (i.InFineMenu == true)
                            {
                                i.InFineMenu = false;
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

                    Conn.AcceptTicket = 0;
                    Conn.IsBeingBusted = false;
                    Conn.CopInChase = 0;
                    Conn.TicketRefuse = 0;

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
                }

                if (Conn.InChaseProgress == true)
                {
                    if (ChaseCon.CopInChase > 1)
                    {
                        if (Conn.JoinedChase == true)
                        {
                            Conn.JoinedChase = false;
                        }
                        Conn.ChaseCondition = 0;
                        Conn.Busted = false;
                        Conn.BustedTimer = 0;
                        Conn.BumpButton = 0;
                        Conn.Chasee = -1;
                        ChaseCon.CopInChase -= 1;

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

                        MsgAll("^9 " + Conn.PlayerName + " sighting lost " + ChaseCon.PlayerName + "!");
                        MsgAll("   ^7 Total Cops In Chase: " + ChaseCon.CopInChase);
                    }
                    else if (ChaseCon.CopInChase == 1)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " lost " + ChaseCon.PlayerName + "!");
                        MsgAll("   ^7Suspect Runs away from being chased!");
                        AddChaseLimit -= 1;
                        Conn.AutoBumpTimer = 0;
                        Conn.BumpButton = 0;
                        Conn.BustedTimer = 0;
                        Conn.Chasee = -1;
                        Conn.Busted = false;
                        ChaseCon.PullOvrMsg = 0;
                        ChaseCon.ChaseCondition = 0;
                        ChaseCon.CopInChase = 0;
                        ChaseCon.IsSuspect = false;
                        Conn.ChaseCondition = 0;
                        CopSirenShutOff();
                    }

                    Conn.InChaseProgress = false;
                }

                #endregion

                #region ' Tow System '

                if (Conn.InTowProgress == true)
                {
                    if (TowCon.IsBeingTowed == true)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " stopped towing " + TowCon.PlayerName + "!");
                        TowCon.IsBeingTowed = false;
                    }
                    Conn.Towee = -1;
                    Conn.InTowProgress = false;
                    CautionSirenShutOff();
                }

                if (Conn.IsBeingTowed == true)
                {
                    MsgAll("^9 " + Conn.PlayerName + " pitted whilst being Towed!");
                    foreach (clsConnection o in Connections)
                    {
                        if (o.Towee == Conn.UniqueID)
                        {
                            o.InTowProgress = false;
                            o.Towee = -1;
                        }
                    }
                    Conn.IsBeingTowed = false;
                    CautionSirenShutOff();
                }

                #endregion

                #region ' Return Rent '
                if (Conn.Rented == 1)
                {
                    bool Found = false;

                    #region ' Online '
                    foreach (clsConnection C in Connections)
                    {
                        if (C.Username == Conn.Rentee)
                        {
                            Found = true;
                            C.Renting = 0;
                            C.Renter = "0";
                            MsgAll("^9 " + Conn.PlayerName + " their rentals returned to " + C.PlayerName);
                        }
                    }
                    #endregion

                    #region ' Offline '
                    if (Found == false)
                    {
                        #region ' Objects '

                        long Cash = FileInfo.GetUserCash(Conn.Rentee);
                        long BBal = FileInfo.GetUserBank(Conn.Rentee);
                        string Cars = FileInfo.GetUserCars(Conn.Rentee);
                        long Gold = FileInfo.GetUserGold(Conn.Rentee);

                        long TotalDistance = FileInfo.GetUserDistance(Conn.Rentee);
                        byte TotalHealth = FileInfo.GetUserHealth(Conn.Rentee);
                        int TotalJobsDone = FileInfo.GetUserJobsDone(Conn.Rentee);

                        byte Electronics = FileInfo.GetUserElectronics(Conn.Rentee);
                        byte Furniture = FileInfo.GetUserFurniture(Conn.Rentee);

                        int LastRaffle = FileInfo.GetUserLastRaffle(Conn.Rentee);
                        int LastLotto = FileInfo.GetUserLastLotto(Conn.Rentee);

                        byte CanBeOfficer = FileInfo.CanBeOfficer(Conn.Rentee);
                        byte CanBeCadet = FileInfo.CanBeCadet(Conn.Rentee);
                        byte CanBeTowTruck = FileInfo.CanBeTowTruck(Conn.Rentee);
                        byte IsModerator = FileInfo.IsMember(Conn.Rentee);

                        byte Interface1 = FileInfo.GetInterface(Conn.Rentee);
                        byte Interface2 = FileInfo.GetInterface2(Conn.Rentee);
                        byte Speedo = FileInfo.GetSpeedo(Conn.Rentee);
                        byte Odometer = FileInfo.GetOdometer(Conn.Rentee);
                        byte Counter = FileInfo.GetCounter(Conn.Rentee);
                        byte Panel = FileInfo.GetCopPanel(Conn.Rentee);

                        byte Renting = FileInfo.GetUserRenting(Conn.Rentee);
                        byte Rented = FileInfo.GetUserRented(Conn.Rentee);
                        string Renter = FileInfo.GetUserRenter(Conn.Rentee);
                        string Renterr = FileInfo.GetUserRenterr(Conn.Rentee);
                        string Rentee = FileInfo.GetUserRentee(Conn.Rentee);

                        string PlayerName = FileInfo.GetUserPlayerName(Conn.Rentee);
                        #endregion

                        #region ' Remove Renting '

                        Renting = 0;
                        Renter = "0";

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

                        MsgAll("^9 " + Conn.PlayerName + " their rentals returned to " + PlayerName);

                        #region ' Save User '

                        FileInfo.SaveOfflineUser(Conn.Rentee,
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
                    #endregion

                    Conn.Rentee = "0";
                    Conn.Rented = 0;
                }
                #endregion

                Conn.TripMeter = 0;
                Conn.Location = "Fix 'N' Repair Station";
                Conn.LastSeen = "Fix 'N' Repair Station";
                Conn.LocationBox = "^7Fix 'N' Repair Station";
                Conn.SpeedBox = "";
                
                // Update Player List[]
                Conn.PlayerID = 0;
                Conn.CompCar = new Packets.CompCar();
            }
            catch { }
        }

        // Car contact between 2 cars
        private void CON_CarContact(Packets.IS_CON CON)
        {
            try
            {

                var ConnA = Connections[GetConnIdx2(CON.A.PLID)];
                var ConnB = Connections[GetConnIdx2(CON.B.PLID)];
                var kmhA = ConnA.CompCar.Speed / 91;
                var kmhB = ConnB.CompCar.Speed / 91;

                #region 'Car Contact Driving[GT] By FOX'
                if (kmhA > kmhB)
                {

                    if (ConnA.IsOfficer == false && ConnA.IsCadet == false && ConnA.IsTowTruck == false)
                    {
                        if (ConnA.IsSuspect == false && ConnA.IsBeingTowed == false && ConnA.InTowProgress == false)
                        {
                           
                            MsgPly("^1 + 1 % Car Demage!", ConnA.UniqueID);
                            MsgPly("^1 + 1 % Car Demage!", ConnB.UniqueID);
                            ConnA.TotalHealth += 1;
                            ConnB.TotalHealth += 1;
                            kmhA = 0;
                            kmhB = 0;
                            Random crps = new Random();
                            int fncrps = crps.Next(25, 250);
                            ConnA.Cash -= fncrps;
                            ConnB.Cash -= fncrps;
                            MsgPly("^1Fine: ^7" + fncrps.ToString() + "^7$", ConnA.UniqueID);
                            MsgPly("^1Fine: ^7" + fncrps.ToString() + "^7$", ConnB.UniqueID);
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                    }




                }
                else if (kmhA < kmhB)
                {

                    if (ConnB.IsOfficer == false && ConnB.IsCadet == false && ConnB.IsTowTruck == false)
                    {
                        if (ConnB.IsSuspect == false && ConnB.IsBeingTowed == false && ConnB.InTowProgress == false)
                        {
                           
                            
                            MsgPly("^1 + 1 % Car Demage!", ConnB.UniqueID);
                            MsgPly("^1 + 1 % Car Demage!", ConnA.UniqueID);
                            ConnB.TotalHealth += 1;
                            ConnA.TotalHealth += 1;
                            kmhA = 0;
                            kmhB = 0;




                        }

                        else
                        {

                        }
                    }
                    else
                    {

                    }

                }







                #endregion

            }
            catch { }
        }
        

        // A player joins the race. If PLID already exists, then player leaves pit.
        private void NPL_PlayerJoinsRace(Packets.IS_NPL NPL)
        {
            try
            {
                var Conn = Connections[GetConnIdx(NPL.UCID)];

                #region ' Check User cars '

                if (Conn.Rented == 0)
                {
                    if (Conn.Cars.Contains(NPL.CName) != true)
                    {
                        int RandomFines = new Random().Next(200, 250);
                        SpecID(Conn.Username);
                        SpecID(Conn.PlayerName);
                        MsgAll("^9 " + Conn.NoColPlyName + " tried to steal ^3" + NPL.CName);
                        MsgAll(" ^7but was caught and fined for ^1$" + RandomFines);
                        Conn.Cash -= RandomFines;

                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        Conn.StealTime += 1;


                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else if (NPL.CName == "FBM" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 10000)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^9 You need 10000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        { //H
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        if (NPL.CName == "LX4" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 500)
                        {
                            MsgPly("^9 You need 500KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }
                        else if (NPL.CName == "LX6" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 800)
                        {
                            MsgPly("^9 You need 800KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }

                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        Conn.StealTime += 1;
                    }
                    else if (NPL.CName == "LX4" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 500)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^9 You need 500KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else if (NPL.CName == "LX6" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 800)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^9 You need 800KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        } //H
                        if (NPL.CName == "RB4" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 900)
                        {
                            MsgPly("^9 You need 900KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }
                        else if (NPL.CName == "FXO" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 1200)
                        {
                            MsgPly("^9 You need 1200KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }

                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        Conn.StealTime += 1;
                    }
                    else if (NPL.CName == "RB4" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 900)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^9 You need 900KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else if (NPL.CName == "FXO" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 1200)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^9 You need 1200KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        { //H
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        if (NPL.CName == "XRT" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 1600)
                        {
                            MsgPly("^9 You need 1600KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }
                        else if (NPL.CName == "RAC" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 1800)
                        {
                            MsgPly("^9 You need 1800KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }

                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        Conn.StealTime += 1;
                    }
                    else if (NPL.CName == "XRT" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 1600)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^9 You need 1600KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else if (NPL.CName == "RAC" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 1800)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^9 You need 1800KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        {
                        }
                        if (NPL.CName == "FZ5" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 2000)
                        {
                            MsgPly("^9 You need 2000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }
                        else if (NPL.CName == "UFR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 2200)
                        {
                            MsgPly("^9 You need 2200KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }

                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        Conn.StealTime += 1;
                    }
                    else if (NPL.CName == "FZ5" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 2000)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^9 You need 2000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else if (NPL.CName == "UFR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 2200)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^9 You need 2200KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        if (NPL.CName == "XFR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 2400)
                        {
                            MsgPly("^9 You need 2400KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }
                        else if (NPL.CName == "FXR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 3000)
                        {
                            MsgPly("^9 You need 3000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }

                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        Conn.StealTime += 1;
                    }
                    else if (NPL.CName == "XFR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 2400)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^9 You need 2400KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else if (NPL.CName == "FXR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 3000)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^9 You need 3000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        if (NPL.CName == "XRR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 3500)
                        {
                            MsgPly("^9 You need 3500KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }
                        else if (NPL.CName == "FZR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 4000)
                        {
                            MsgPly("^9 You need 4000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }

                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        Conn.StealTime += 1;
                    }
                    else if (NPL.CName == "XRR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 3500)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^9 You need 3500KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else if (NPL.CName == "FZR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 4000)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^9 You need 4000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        if (NPL.CName == "MRT" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 1000)
                        {
                            MsgPly("^9 You need 1000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }
                        else if (NPL.CName == "" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 4000)
                        {
                            MsgPly("^9 You need 4000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }

                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        Conn.StealTime += 1;
                    }
                    else if (NPL.CName == "MRT" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 1000)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^9 You need 1000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else if (NPL.CName == "" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 0)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^9 You need 0KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^9 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else
                    {

                        Conn.LeavesPitLane = 1;
                        Conn.InGame = 1;
                    }
                }
                else
                {
                    Conn.LeavesPitLane = 1;
                    Conn.InGame = 1;
                }
                #endregion

                if (Connections[GetConnIdx(NPL.UCID)].Cash < 0)
                {
                    InSim.Send_MTC_MessageToConnection("-------------------", NPL.UCID, 0);
                    InSim.Send_MTC_MessageToConnection("^1Your networth has reached a negative value.", NPL.UCID, 0);
                    InSim.Send_MTC_MessageToConnection("^1If your networth reaches ^1-$50 000 ^9you will banned.", NPL.UCID, 0);
                    InSim.Send_MTC_MessageToConnection("-------------------", NPL.UCID, 0);
                }

                if (Conn.StealTime == 4)
                {
                    MsgAll("^9 " + Conn.PlayerName + " was kicked for stealing.");
                    KickID(Conn.Username);
                }

                #region ' Check Admin '

                if (NPL.PName == HostName == false)
                {
                    if (Conn.IsAdmin == 1)
                    {
                        if (Conn.IsSuperAdmin == 0)
                        {
                            MsgAll("^9 " + Conn.PlayerName + " is not a admin!");
                            SpecID(NPL.PName);
                        }
                    }
                }
                #endregion

                


                if (Conn.CanBeTowTruck == 1 && Conn.IsTowTruck == true)
                {
                    if (NPL.CName == "FBM")
                    {
                        MsgAll("^9 " + Conn.PlayerName + " is now ^1OFF-DUTY ^7as Tow Truck!");
                        Conn.IsTowTruck = false;
                    }

                }

                // Below Updates
                Connections[GetConnIdx(NPL.UCID)].Plate = NPL.Plate;

                Connections[GetConnIdx(NPL.UCID)].SkinName = NPL.SName;

                Connections[GetConnIdx(NPL.UCID)].CurrentCar = NPL.CName;

                Connections[GetConnIdx(NPL.UCID)].PlayerPacket = NPL;

                Connections[GetConnIdx(NPL.UCID)].PlayerID = NPL.PLID;	// Update Players[] list
            }
            catch { }
        }

        // A player resets the car
        private void CRS_PlayerResetsCar(Packets.IS_CRS CRS)
        {
            try
            {
                
            }
            catch { }
        }

        // A player stops for making a pitstop
        private void PIT_PlayerStopsAtPit(Packets.IS_PIT PIT)
        {
            try
            {
                #region ' UniqueID Loader '
                int IDX = -1;
                for (int i = 0; i < Connections.Count; i++)
                {
                    if (Connections[i].PlayerID == PIT.PLID)
                    {
                        IDX = i;
                        break;
                    }
                }
                if (IDX == -1)
                    return;
                clsConnection Conn = Connections[IDX];
                #endregion

            }
            catch { }
        }

        // A penalty give or cleared
        private void PEN_PenaltyChanged(Packets.IS_PEN PEN)
        {    
		    try
            {
                #region ' UniqueID Loader '
                int IDX = -1;
                for (int i = 0; i < Connections.Count; i++)
                {
                    if (Connections[i].PlayerID == PEN.PLID)
                    {
                        IDX = i;
                        break;
                    }
                }
                if (IDX == -1)
                    return;
                clsConnection Conn = Connections[IDX];
                #endregion
            }
            catch { }
        }

        // The server/race state changed
        private void STA_StateChanged(Packets.IS_STA STA)
        {
            try
            {
                TrackName = STA.Track;
            }
            catch { }
        }

        // A host ends or leaves
        private void MPE_MultiplayerEnd()
        {			
		    try
            {
                foreach (clsConnection C in Connections)
				{
					var Conn = Connections[GetConnIdx(C.UniqueID)];
				}
            }
            catch { }
        }

        // Sent at the start of every race or qualifying session, listing the start order
        private void REO_RaceStartOrder(Packets.IS_REO REO)
        {
			try
            {
                

            }
            catch { }
        }

        // Race start information
        private void RST_RaceStart(Packets.IS_RST RST)
        {
			try
            {

            }
            catch { }
        }

        // A race ends (return to game setup screen)
        private void REN_RaceEnds()
        {
			try
            {
                foreach (clsConnection C in Connections)
				{
					var Conn = Connections[GetConnIdx(C.UniqueID)];
				}
            }
            catch { }
        }

        // A player submitted a custom textbox
        private void BTT_TextBoxOkClicked(Packets.IS_BTT BTT)
        {
            try
            {
                clsConnection Conn = Connections[GetConnIdx(BTT.UCID)];

                #region ' Cop Panel '

                if (Conn.IsOfficer == true && Conn.CopPanel == 1 && BTT.ClickID == 21)
                {
                    if (Conn.InChaseProgress == false)
                    {
                        try
                        {
                            int TrapSpeed = Convert.ToInt32(BTT.Text);

                            if (TrapSpeed.ToString().Contains("-"))
                            {
                                MsgPly("^9 Invalid Input. Don't put minus values!", BTT.UCID);
                            }
                            else
                            {
                                if (Conn.TrapSetted == false)
                                {
                                    if (Conn.CompCar.Speed / 91 < 3)
                                    {
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
                                        MsgPly("^9 Can't Set a Trap whilst being driving!", BTT.UCID);
                                    }
                                }
                                else
                                {
                                    MsgPly("^9 The Trap has been setted in this Area!", BTT.UCID);
                                }
                            }
                        }
                        catch
                        {
                            MsgPly("^9 Trap Error. Please check your values!", BTT.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^9 Can't set a Trap whilst in chase progress!", BTT.UCID);
                    }
                }

                #endregion

                #region ' Busted Panel '

                if (Conn.InFineMenu == true)
                {
                    switch (BTT.ClickID)
                    {
                        #region ' Reason Ticket '
                        case 36:
                            string Reason = (BTT.Text);
                            if (Reason.Length >= 0)
                            {
                                InSim.Send_BTN_CreateButton("^7Reason: " + Reason, "Enter the chase reason", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 78, 77, 64, 36, (Conn.UniqueID), 40, false);
                                Conn.TicketReason = Reason;
                                Conn.TicketReasonSet = true;
                            }
                            else
                            {
                                MsgPly("^9 Input Error. You must fill the Reason!", BTT.UCID);
                                Conn.TicketReason = "";
                                Conn.TicketReasonSet = false;
                                InSim.Send_BTN_CreateButton("^7Reason", "Enter the chase reason", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 78, 77, 64, 36, (Conn.UniqueID), 40, false);
                            }
                            break;
                        #endregion

                        #region ' Fine Amount Ticket '
                        case 37:
                            int Amount = int.Parse(BTT.Text);
                            bool Complete = false;
                            if (BTT.Text.Contains("-"))
                            {
                                MsgPly("^9 Deposit Incorrect. Don't put minus on the values!", BTT.UCID);
                            }
                            else if (BTT.Text.Length < 0)
                            {
                                MsgPly("^9 Fine Error. Invalid input!", BTT.UCID);
                                InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                            }
                            else if (Amount == 0)
                            {
                                MsgPly("^9 Fine Error. The Fine must have a value!", BTT.UCID);
                                InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                            }
                            else
                            {
                                if (BTT.Text != "")
                                {
                                    if (Conn.ChaseCondition == 1)
                                    {
                                        if (Amount > 700)
                                        {
                                            MsgPly("^9 Fine Error. Too high for the Maximum Fine in this Condition!", BTT.UCID);
                                            InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                                        }
                                        else
                                        {
                                            Complete = true;
                                        }
                                    }
                                    else if (Conn.ChaseCondition == 2)
                                    {
                                        if (Amount > 1300)
                                        {
                                            MsgPly("^9 Fine Error. Too high for the Maximum Fine in this Condition!", BTT.UCID);
                                            InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                                        }
                                        else
                                        {
                                            Complete = true;
                                        }
                                    }
                                    else if (Conn.ChaseCondition == 3)
                                    {
                                        if (Amount > 2500)
                                        {
                                            MsgPly("^9 Fine Error. Too high for the Maximum Fine in this Condition!", BTT.UCID);
                                            InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                                        }
                                        else
                                        {
                                            Complete = true;
                                        }
                                    }
                                    else if (Conn.ChaseCondition == 4)
                                    {
                                        if (Amount > 3500)
                                        {
                                            MsgPly("^9 Fine Error. Too high for the Maximum Fine in this Condition!", BTT.UCID);
                                            InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                                        }
                                        else
                                        {
                                            Complete = true;
                                        }
                                    }
                                    else if (Conn.ChaseCondition == 5)
                                    {
                                        if (Amount > 5000)
                                        {
                                            MsgPly("^9 Fine Error. Too high for the Maximum Fine in this Condition!", BTT.UCID);
                                            InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                                        }
                                        else
                                        {
                                            Complete = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (Conn.TicketAmountSet == true)
                                    {
                                        Conn.TicketAmount = 0;
                                        Conn.TicketAmountSet = false;
                                        InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                                    }
                                }
                            }

                            if (Complete == true)
                            {
                                Conn.TicketAmount = Amount;
                                Conn.TicketAmountSet = true;
                                InSim.Send_BTN_CreateButton("^7Fine ^1$" + Amount, "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                            }
                            break;
                        #endregion
                    }
                }

                #endregion

                #region ' Moderation Panel '

                if (Conn.InModerationMenu == 1)
                {
                    switch (BTT.ClickID)
                    {
                        #region ' Reason Window '
                        case 37:
                            string Reason = BTT.Text;
                            if (Reason.Length > 0)
                            {
                                InSim.Send_BTN_CreateButton("^4>> ^7Reason: " + Reason + " ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                InSim.Send_BTN_CreateButton("^7WARN", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 54, 38, Conn.UniqueID, 40, false);
                                InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                InSim.Send_BTN_CreateButton("^7SPEC", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 82, 40, Conn.UniqueID, 40, false);
                                InSim.Send_BTN_CreateButton("^7KICK", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 96, 41, Conn.UniqueID, 40, false);
                                InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                Conn.ModReason = Reason;
                                Conn.ModReasonSet = true;
                            }
                            else
                            {
                                InSim.Send_BTN_CreateButton("^4>> ^7Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^8FINE", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 68, 39, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^8BAN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 110, 42, Conn.UniqueID, 2, false);
                                Conn.ModReason = "";
                                Conn.ModReasonSet = false;
                            }

                            break;
                        #endregion

                        #region ' Fine '
                        case 39:
                            
                            if (Conn.ModReasonSet == true)
                            {
                                bool Found = false;
                                int Amount = int.Parse(BTT.Text);

                                if (Amount.ToString().Contains("-"))
                                {
                                    MsgPly("^9 Input Invalid. Don't use minus on the Values!", BTT.UCID);
                                }
                                else
                                {
                                    #region ' Online '
                                    foreach (clsConnection i in Connections)
                                    {
                                        if (i.Username == Conn.ModUsername)
                                        {
                                            Found = true;
                                            MsgAll("^9 " + i.PlayerName + " was force fined for ^1$" + Amount);
                                            MsgAll("^9 Reason: " + Conn.ModReason);
                                            MsgPly("> You are fined by " + Conn.PlayerName, i.UniqueID);
                                            
                                            i.Cash -= Amount;
                                        }
                                    }
                                    #endregion

                                    #region ' Found '
                                    if (Found == true)
                                    {
                                        InSim.Send_BTN_CreateButton("^4>> ^7Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                        InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8FINE", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 68, 39, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8BAN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 110, 42, Conn.UniqueID, 2, false);
                                    }
                                    #endregion

                                    #region ' Offline '
                                    else if (Found == false)
                                    {
                                        if (System.IO.File.Exists(Database + "\\" + Conn.ModUsername + ".txt") == true)
                                        {
                                            #region ' Objects '
                                            long Cash = FileInfo.GetUserCash(Conn.ModUsername);
                                            long BBal = FileInfo.GetUserBank(Conn.ModUsername);
                                            string Cars = FileInfo.GetUserCars(Conn.ModUsername);
                                            long Gold = FileInfo.GetUserGold(Conn.ModUsername);

                                            long TotalDistance = FileInfo.GetUserDistance(Conn.ModUsername);
                                            byte TotalHealth = FileInfo.GetUserHealth(Conn.ModUsername);
                                            int TotalJobsDone = FileInfo.GetUserJobsDone(Conn.ModUsername);

                                            byte Electronics = FileInfo.GetUserElectronics(Conn.ModUsername);
                                            byte Furniture = FileInfo.GetUserFurniture(Conn.ModUsername);

                                            int LastRaffle = FileInfo.GetUserLastRaffle(Conn.ModUsername);
                                            int LastLotto = FileInfo.GetUserLastLotto(Conn.ModUsername);

                                            byte CanBeOfficer = FileInfo.CanBeOfficer(Conn.ModUsername);
                                            byte CanBeCadet = FileInfo.CanBeCadet(Conn.ModUsername);
                                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Conn.ModUsername);
                                            byte IsModerator = FileInfo.IsMember(Conn.ModUsername);

                                            byte Interface1 = FileInfo.GetInterface(Conn.ModUsername);
                                            byte Interface2 = FileInfo.GetInterface2(Conn.ModUsername);
                                            byte Speedo = FileInfo.GetSpeedo(Conn.ModUsername);
                                            byte Odometer = FileInfo.GetOdometer(Conn.ModUsername);
                                            byte Counter = FileInfo.GetCounter(Conn.ModUsername);
                                            byte Panel = FileInfo.GetCopPanel(Conn.ModUsername);

                                            byte Renting = FileInfo.GetUserRenting(Conn.ModUsername);
                                            byte Rented = FileInfo.GetUserRented(Conn.ModUsername);
                                            string Renter = FileInfo.GetUserRenter(Conn.ModUsername);
                                            string Renterr = FileInfo.GetUserRenterr(Conn.ModUsername);
                                            string Rentee = FileInfo.GetUserRentee(Conn.ModUsername);

                                            string PlayerName = FileInfo.GetUserPlayerName(Conn.ModUsername);
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

                                            // Your Code here
                                            MsgAll("^9 " + PlayerName + " was force fined for ^1$" + Amount);
                                            MsgAll("^9 Reason: " + Conn.ModReason);
                                           
                                            Cash -= Amount;

                                            InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                            InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                            InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                        

                                            Conn.InModerationMenu = 2;

                                            #region ' Save User '
                                            FileInfo.SaveOfflineUser(Conn.ModUsername,
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
                                            MsgPly("^9 " + Conn.ModUsername + " wasn't found on database", BTT.UCID);
                                        }
                                    }
                                    #endregion

                                    Conn.ModReason = "";
                                    Conn.ModReasonSet = false;
                                }
                            }
                            else
                            {
                                MsgPly("^9 Reason not yet setted.", BTT.UCID);
                            }

                            break;
                        #endregion

                        #region ' Ban '
                        case 42:

                            if (Conn.ModReasonSet == true)
                            {
                                int Days = int.Parse(BTT.Text);
                                bool Found = false;

                                if (Days.ToString().Contains("-"))
                                {
                                    MsgPly("^9 Input Invalid. Don't use minus on the Values!", BTT.UCID);
                                }
                                else
                                {
                                    #region ' Online '

                                    foreach (clsConnection i in Connections)
                                    {
                                        if (i.Username == Conn.ModUsername)
                                        {
                                            MsgAll("^9 " + i.PlayerName + " (" + i.Username + ") was banned.");
                                            MsgPly("> You are banned by " + Conn.PlayerName, i.UniqueID);
                                            MsgAll("^9 Reason: " + Conn.ModReason);
                                            
                                            
                                            if (Days == 0)
                                            {
                                                MsgPly("^9 You are banned for 12 hours", i.UniqueID);
                                            }
                                            else if (Days == 1)
                                            {
                                                MsgPly("^9 You are banned for " + Days + " Day", i.UniqueID);
                                            }
                                            else
                                            {
                                                MsgPly("^9 You are banned for " + Days + " Days", i.UniqueID);
                                            }

                                            BanID(i.Username, Days);
                                        }
                                    }

                                    if (Found == true)
                                    {
                                        InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                        InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                        InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                        Conn.ModerationWarn = 2;
                                    }

                                    #endregion

                                    #region ' Offline '

                                    else if (Found == false)
                                    {
                                        #region ' Objects '
                                        long Cash = FileInfo.GetUserCash(Conn.ModUsername);
                                        long BBal = FileInfo.GetUserBank(Conn.ModUsername);
                                        string Cars = FileInfo.GetUserCars(Conn.ModUsername);
                                        long Gold = FileInfo.GetUserGold(Conn.ModUsername);

                                        long TotalDistance = FileInfo.GetUserDistance(Conn.ModUsername);
                                        byte TotalHealth = FileInfo.GetUserHealth(Conn.ModUsername);
                                        int TotalJobsDone = FileInfo.GetUserJobsDone(Conn.ModUsername);

                                        byte Electronics = FileInfo.GetUserElectronics(Conn.ModUsername);
                                        byte Furniture = FileInfo.GetUserFurniture(Conn.ModUsername);

                                        int LastRaffle = FileInfo.GetUserLastRaffle(Conn.ModUsername);
                                        int LastLotto = FileInfo.GetUserLastLotto(Conn.ModUsername);

                                        byte CanBeOfficer = FileInfo.CanBeOfficer(Conn.ModUsername);
                                        byte CanBeCadet = FileInfo.CanBeCadet(Conn.ModUsername);
                                        byte CanBeTowTruck = FileInfo.CanBeTowTruck(Conn.ModUsername);
                                        byte IsModerator = FileInfo.IsMember(Conn.ModUsername);

                                        byte Interface1 = FileInfo.GetInterface(Conn.ModUsername);
                                        byte Interface2 = FileInfo.GetInterface2(Conn.ModUsername);
                                        byte Speedo = FileInfo.GetSpeedo(Conn.ModUsername);
                                        byte Odometer = FileInfo.GetOdometer(Conn.ModUsername);
                                        byte Counter = FileInfo.GetCounter(Conn.ModUsername);
                                        byte Panel = FileInfo.GetCopPanel(Conn.ModUsername);

                                        byte Renting = FileInfo.GetUserRenting(Conn.ModUsername);
                                        byte Rented = FileInfo.GetUserRented(Conn.ModUsername);
                                        string Renter = FileInfo.GetUserRenter(Conn.ModUsername);
                                        string Renterr = FileInfo.GetUserRenterr(Conn.ModUsername);
                                        string Rentee = FileInfo.GetUserRentee(Conn.ModUsername);

                                        string PlayerName = FileInfo.GetUserPlayerName(Conn.ModUsername);
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

                                        MsgAll("^9 " + PlayerName + " (" + Conn.ModUsername + ") was banned.");
                                        MsgAll("^9 Reason: " + Conn.ModReason);

                                        
                                        BanID(Conn.ModUsername, Days);

                                        InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                        InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                        InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                        Conn.ModerationWarn = 2;
                                    }

                                    #endregion

                                    Conn.ModReason = "";
                                    Conn.ModReasonSet = false;
                                }
                            }
                            else
                            {
                                MsgPly("^9 Reason not yet setted.", BTT.UCID);
                            }

                            break;
                        #endregion
                    }
                }

                else if (Conn.InModerationMenu == 2)
                {
                    switch (BTT.ClickID)
                    {
                        #region ' Fine '
                        case 39:

                                bool Found = false;
                                int Amount = int.Parse(BTT.Text);

                                if (Amount.ToString().Contains("-"))
                                {
                                    MsgPly("^9 Input Invalid. Don't use minus on the Values!", BTT.UCID);
                                }
                                else
                                {
                                    #region ' Online '
                                    foreach (clsConnection i in Connections)
                                    {
                                        if (i.Username == Conn.ModUsername)
                                        {
                                            Found = true;
                                            MsgAll("^9 " + i.PlayerName + " was force fined for ^1$" + Amount);
                                            MsgPly("> You are fined by " + Conn.PlayerName, i.UniqueID);
                                            //AdmBox("> " + Conn.PlayerName + " fined " + i.PlayerName + " (" + i.Username + ") for $" + Amount + "!");
                                            i.Cash -= Amount;
                                        }
                                    }
                                    #endregion

                                    #region ' Found '
                                    if (Found == true)
                                    {
                                        InSim.Send_BTN_CreateButton("^4>> ^7Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                        InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8FINE", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 68, 39, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8BAN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 110, 42, Conn.UniqueID, 2, false);
                                        Conn.InModerationMenu = 1;
                                    }
                                    #endregion

                                    #region ' Offline '
                                    else if (Found == false)
                                    {
                                        if (System.IO.File.Exists(Database + "\\" + Conn.ModUsername + ".txt") == true)
                                        {
                                            #region ' Objects '
                                            long Cash = FileInfo.GetUserCash(Conn.ModUsername);
                                            long BBal = FileInfo.GetUserBank(Conn.ModUsername);
                                            string Cars = FileInfo.GetUserCars(Conn.ModUsername);
                                            long Gold = FileInfo.GetUserGold(Conn.ModUsername);

                                            long TotalDistance = FileInfo.GetUserDistance(Conn.ModUsername);
                                            byte TotalHealth = FileInfo.GetUserHealth(Conn.ModUsername);
                                            int TotalJobsDone = FileInfo.GetUserJobsDone(Conn.ModUsername);

                                            byte Electronics = FileInfo.GetUserElectronics(Conn.ModUsername);
                                            byte Furniture = FileInfo.GetUserFurniture(Conn.ModUsername);

                                            int LastRaffle = FileInfo.GetUserLastRaffle(Conn.ModUsername);
                                            int LastLotto = FileInfo.GetUserLastLotto(Conn.ModUsername);

                                            byte CanBeOfficer = FileInfo.CanBeOfficer(Conn.ModUsername);
                                            byte CanBeCadet = FileInfo.CanBeCadet(Conn.ModUsername);
                                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Conn.ModUsername);
                                            byte IsModerator = FileInfo.IsMember(Conn.ModUsername);

                                            byte Interface1 = FileInfo.GetInterface(Conn.ModUsername);
                                            byte Interface2 = FileInfo.GetInterface2(Conn.ModUsername);
                                            byte Speedo = FileInfo.GetSpeedo(Conn.ModUsername);
                                            byte Odometer = FileInfo.GetOdometer(Conn.ModUsername);
                                            byte Counter = FileInfo.GetCounter(Conn.ModUsername);
                                            byte Panel = FileInfo.GetCopPanel(Conn.ModUsername);

                                            byte Renting = FileInfo.GetUserRenting(Conn.ModUsername);
                                            byte Rented = FileInfo.GetUserRented(Conn.ModUsername);
                                            string Renter = FileInfo.GetUserRenter(Conn.ModUsername);
                                            string Renterr = FileInfo.GetUserRenterr(Conn.ModUsername);
                                            string Rentee = FileInfo.GetUserRentee(Conn.ModUsername);

                                            string PlayerName = FileInfo.GetUserPlayerName(Conn.ModUsername);
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

                                            // Your Code here
                                            MsgAll("^9 " + PlayerName + " was force fined for ^1$" + Amount);
                                            //AdmBox("> " + Conn.PlayerName + " fined " + PlayerName + " (" + Conn.ModUsername + ") for $" + Amount + "!");
                                            Cash -= Amount;

                                            InSim.Send_BTN_CreateButton("^4>> ^7Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                            InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                            InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                            Conn.InModerationMenu = 2;

                                            #region ' Save User '
                                            FileInfo.SaveOfflineUser(Conn.ModUsername,
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
                                            MsgPly("^9 " + Conn.ModUsername + " wasn't found on database", BTT.UCID);
                                        }
                                    }
                                    #endregion

                                    Conn.ModReason = "";
                                    Conn.ModReasonSet = false;
                                }

                            break;
                        #endregion

                        #region ' Ban '
                        case 42:
                                int Days = int.Parse(BTT.Text);
                                bool Found1 = false;
                                if (Days.ToString().Contains("-"))
                                {
                                    MsgPly("^9 Input Invalid. Don't use minus on the Values!", BTT.UCID);
                                }
                                else
                                {
                                    #region ' Online '

                                    foreach (clsConnection i in Connections)
                                    {
                                        if (i.Username == Conn.ModUsername)
                                        {
                                            Found1 = true;
                                            MsgAll("^9 " + i.PlayerName + " (" + i.Username + ") was banned.");
                                            MsgPly("> You are banned by " + Conn.PlayerName, i.UniqueID);
                                            //AdmBox("> " + Conn.PlayerName + " banned " + i.PlayerName + " (" + i.Username + ") for " + Days + "!");
                                            
                                            if (Days == 0)
                                            {
                                                MsgPly("^9 You are banned for 12 hours", i.UniqueID);
                                            }
                                            else if (Days == 1)
                                            {
                                                MsgPly("^9 You are banned for " + Days + " Day", i.UniqueID);
                                            }
                                            else
                                            {
                                                MsgPly("^9 You are banned for " + Days + " Days", i.UniqueID);
                                            }

                                            BanID(i.Username, Days);
                                        }
                                    }

                                    if (Found1 == true)
                                    {
                                        InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                        InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                        InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                        Conn.ModerationWarn = 2;
                                    }

                                    #endregion

                                    #region ' Offline '

                                    else if (Found1 == false)
                                    {
                                        #region ' Objects '
                                        long Cash = FileInfo.GetUserCash(Conn.ModUsername);
                                        long BBal = FileInfo.GetUserBank(Conn.ModUsername);
                                        string Cars = FileInfo.GetUserCars(Conn.ModUsername);
                                        long Gold = FileInfo.GetUserGold(Conn.ModUsername);

                                        long TotalDistance = FileInfo.GetUserDistance(Conn.ModUsername);
                                        byte TotalHealth = FileInfo.GetUserHealth(Conn.ModUsername);
                                        int TotalJobsDone = FileInfo.GetUserJobsDone(Conn.ModUsername);

                                        byte Electronics = FileInfo.GetUserElectronics(Conn.ModUsername);
                                        byte Furniture = FileInfo.GetUserFurniture(Conn.ModUsername);

                                        int LastRaffle = FileInfo.GetUserLastRaffle(Conn.ModUsername);
                                        int LastLotto = FileInfo.GetUserLastLotto(Conn.ModUsername);

                                        byte CanBeOfficer = FileInfo.CanBeOfficer(Conn.ModUsername);
                                        byte CanBeCadet = FileInfo.CanBeCadet(Conn.ModUsername);
                                        byte CanBeTowTruck = FileInfo.CanBeTowTruck(Conn.ModUsername);
                                        byte IsModerator = FileInfo.IsMember(Conn.ModUsername);

                                        byte Interface1 = FileInfo.GetInterface(Conn.ModUsername);
                                        byte Interface2 = FileInfo.GetInterface2(Conn.ModUsername);
                                        byte Speedo = FileInfo.GetSpeedo(Conn.ModUsername);
                                        byte Odometer = FileInfo.GetOdometer(Conn.ModUsername);
                                        byte Counter = FileInfo.GetCounter(Conn.ModUsername);
                                        byte Panel = FileInfo.GetCopPanel(Conn.ModUsername);

                                        byte Renting = FileInfo.GetUserRenting(Conn.ModUsername);
                                        byte Rented = FileInfo.GetUserRented(Conn.ModUsername);
                                        string Renter = FileInfo.GetUserRenter(Conn.ModUsername);
                                        string Renterr = FileInfo.GetUserRenterr(Conn.ModUsername);
                                        string Rentee = FileInfo.GetUserRentee(Conn.ModUsername);

                                        string PlayerName = FileInfo.GetUserPlayerName(Conn.ModUsername);
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

                                        MsgAll("^9 " + PlayerName + " (" + Conn.ModUsername + ") was banned.");
                                        BanID(Conn.ModUsername, Days);
                                        //AdmBox("> " + Conn.PlayerName + " banned " + PlayerName + " (" + Conn.ModUsername + ") for " + Days + "!");
                                            
                                        InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                        InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                        InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                        Conn.ModerationWarn = 2;
                                    }

                                    #endregion

                                    Conn.ModReason = "";
                                    Conn.ModReasonSet = false;
                                }

                            break;
                        #endregion
                    }
                }

                #endregion

                
            }
            catch { }

        }

        // A player pressed Shift+I or Shift+B
        private void BFN_PlayerRequestsButtons(Packets.IS_BFN BFN)
        {
            try
            {
                var Conn = Connections[GetConnIdx(BFN.UCID)];
                var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(BFN.UCID)].Chasee)];
                

                

                

                if (Conn.DisplaysOpen == true)
                {
                    Conn.DisplaysOpen = false;
                }

                // Clear Moderator Panel
                if (Conn.InModerationMenu > 0)
                {
                    Conn.ModerationWarn = 0;
                    Conn.ModReason = "";
                    Conn.ModReasonSet = false;
                    Conn.ModUsername = "";
                    Conn.InModerationMenu = 0;
                }

                if (Conn.InFineMenu == true)
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

                #region ' Refuse Pay Fines/Close Warning '
                if (Conn.IsBeingBusted == true)
                {
                    if (Conn.AcceptTicket == 1)
                    {
                        MsgAll("^9 " + Conn.PlayerName + " refused to pay the fines!");
                        MsgAll("  ^7was fined for ^1$5000 ^7for SHIFT+I/B in Ticket Menu!");
                        
                        #region ' Connection List '
                        foreach (clsConnection i in Connections)
                        {
                            if (i.Chasee == Conn.UniqueID)
                            {
                                if (i.InFineMenu == true)
                                {
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

                        Conn.Cash -= 5000;
                        Conn.AcceptTicket = 0;
                        Conn.IsBeingBusted = false;
                        Conn.CopInChase = 0;
                        Conn.TicketRefuse = 0;

                        #region ' Close Region LOL '
                        DeleteBTN(30, BFN.UCID);
                        DeleteBTN(31, BFN.UCID);
                        DeleteBTN(32, BFN.UCID);
                        DeleteBTN(33, BFN.UCID);
                        DeleteBTN(34, BFN.UCID);
                        DeleteBTN(35, BFN.UCID);
                        DeleteBTN(36, BFN.UCID);
                        DeleteBTN(37, BFN.UCID);
                        DeleteBTN(38, BFN.UCID);
                        DeleteBTN(39, BFN.UCID);
                        DeleteBTN(40, BFN.UCID);
                        #endregion
                    }
                }
                if (Conn.AcceptTicket == 2)
                {
                    #region ' Connection List '
                    foreach (clsConnection i in Connections)
                    {
                        if (i.Chasee == Conn.UniqueID)
                        {
                            if (i.InFineMenu == true)
                            {
                                i.InFineMenu = false;
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

                    Conn.AcceptTicket = 0;
                    Conn.IsBeingBusted = false;
                    Conn.CopInChase = 0;
                    Conn.TicketRefuse = 0;

                    #region ' Close Region LOL '
                    DeleteBTN(30, BFN.UCID);
                    DeleteBTN(31, BFN.UCID);
                    DeleteBTN(32, BFN.UCID);
                    DeleteBTN(33, BFN.UCID);
                    DeleteBTN(34, BFN.UCID);
                    DeleteBTN(35, BFN.UCID);
                    DeleteBTN(36, BFN.UCID);
                    DeleteBTN(37, BFN.UCID);
                    DeleteBTN(38, BFN.UCID);
                    DeleteBTN(39, BFN.UCID);
                    DeleteBTN(40, BFN.UCID);
                    #endregion
                }


                #endregion
            }
            catch { }
        }

        // A player clicked a custom button
        private void BTC_ButtonClicked(Packets.IS_BTC BTC)
        {
            try
            {
                BTC_ClientButtonClicked(BTC);                
            }
            catch { }
        }

        // LFS reporting camera position and state
        private void CPP_CameraPosition(Packets.IS_CPP CPP)
        {
            try
            {

            }
            catch { }
        }

        #region ' Unused Packets '
        // A vote got canceled
        private void VTC_VoteCanceled()
        {
            try
            {
                
            }
            catch { }
        }

        // A vote got called
        private void VTN_VoteNotify(Packets.IS_VTN VTN)
        {
		    try
            {
                if (VTN.Action == Enums.VTN_Actions.VOTE_RESTART)
                {
                    
                }
            }
            catch { }
        }
        #endregion

        // Detailed car information packet (max 8 per packet)
        private void MCI_CarInformation(Packets.IS_MCI MCI)
        {
		    try
            {
                int idx = 0;
                for (int i = 0; i < MCI.NumC; i++)
                {
                    idx = GetConnIdx2(MCI.Info[i].PLID); //They aren't structures so you cant serialize!
                    Connections[idx].CompCar.AngVel = MCI.Info[i].AngVel;
                    Connections[idx].CompCar.Direction = MCI.Info[i].Direction;
                    Connections[idx].CompCar.Heading = MCI.Info[i].Heading;
                    Connections[idx].CompCar.Info = MCI.Info[i].Info;
                    Connections[idx].CompCar.Lap = MCI.Info[i].Lap;
                    Connections[idx].CompCar.Node = MCI.Info[i].Node;
                    Connections[idx].CompCar.PLID = MCI.Info[i].PLID;
                    Connections[idx].CompCar.Position = MCI.Info[i].Position;
                    Connections[idx].CompCar.Speed = MCI.Info[i].Speed;
                    Connections[idx].CompCar.X = MCI.Info[i].X;
                    Connections[idx].CompCar.Y = MCI.Info[i].Y;
                    Connections[idx].CompCar.Z = MCI.Info[i].Z;
                }
                for (int i = 0; i < MCI.NumC; i++) //We want everyone to update before checking them.
                {
                    MCI_Update(MCI.Info[i].PLID);
                }
			}
            catch { }
        }

        // InSim version information
        private void VER_InSimVersionInformation(Packets.IS_VER VER)
        {
            try
            {
                if (VER.Product == "DEMO")
                {
                    GameMode = 0;
                }
                if (VER.Product == "S1")
                {
                    GameMode = 1;
                }
                if (VER.Product == "S2")
                {
                    GameMode = 2;
                }
            }
            catch { }
        }

        #endregion

        #region ' Global Buffer '

        string TrackName = "";
        byte OnScreen = 0;
        bool InSimBooted = false;
        byte EndRace = 0;
        //byte Messages = 0;
        byte Stage = 0;
        int RobberUCID = -1;
        bool RentingAllowed = true;
        byte TotalOfficers = 0;
        byte ChaseLimit = 0;
        byte AddChaseLimit = 1;

        #endregion

        #region ' Neat System '

        void CopSirenShutOff()
        {
            try
            {
                foreach (clsConnection C in Connections)
                {
                    if (C.SirenShowned == true)
                    {
                        DeleteBTN(23, C.UniqueID);
                        DeleteBTN(24, C.UniqueID);
                        C.CopSiren = 3;
                        C.SirenShowned = false;
                    }
                }
            }
            catch { }
        }

        void CautionSirenShutOff()
        {
            try
            {
                foreach (clsConnection C in Connections)
                {
                    if (C.SirenShowned == true)
                    {
                        DeleteBTN(23, C.UniqueID);
                        DeleteBTN(24, C.UniqueID);
                        C.TowCautionSiren = 3;
                        C.SirenShowned = false;
                    }
                }
            }
            catch { }
        }

       

        void Message(string MsgStr)
        {
            try
            {
                InSim.Send_MST_Message(MsgStr);
            }
            catch { }
        }

        void MsgAll(string MsgStr)
        {
            try
            {
                Message("/msg " + MsgStr);
            }
            catch { }
        }

        void MsgPly(string MsgStr, byte UNID)
        {
            try
            {
                InSim.Send_MTC_MessageToConnection(MsgStr, UNID, 0);
            }
            catch { }
        }

        void KickID(string Username)
        {
            try
            {
                InSim.Send_MST_Message("/kick " + Username);
            }
            catch { }
        }

        void SpecID(string PlayerName)
        {
            try
            {
                InSim.Send_MST_Message("/spec " + PlayerName);
            }
            catch { }
        }

        void BanID(string Username, int Days)
        {
            try
            {
                InSim.Send_MST_Message("/ban " + Username + " " + Days);
            }
            catch { }
        }

        void PitlaneID(string Username)
        {
            try
            {
                InSim.Send_MST_Message("/pitlane " + Username);
            }
            catch { }
        }

        void ClearPen(string Username)
        {
            try
            {
                InSim.Send_MST_Message("/p_clear " + Username);
            }
            catch { }
        }

        void DeleteBTN(byte ButtonID, byte UNID)
        {
            try
            {
                InSim.Send_BFN_DeleteButton(Enums.BtnFunc.BFN_DEL_BTN, ButtonID, UNID);
            }
            catch { }
        }

        #endregion

        
    }
}