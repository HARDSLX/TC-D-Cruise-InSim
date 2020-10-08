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
        // A player clicked a custom button
        private void BTC_ClientButtonClicked(Packets.IS_BTC BTC)
        {
            try
            {
                var Conn = Connections[GetConnIdx(BTC.UCID)];
                var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(BTC.UCID)].Chasee)];
                string Car = Connections[GetConnIdx(BTC.UCID)].CurrentCar;

                #region ' Close BUTTON! '

                if (BTC.ClickID == 113)
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

                    if (Conn.InAdminMenu == true)
                    {
                        Conn.InAdminMenu = false;
                    }

                    if (Conn.DisplaysOpen == true)
                    {
                        Conn.DisplaysOpen = false;
                    }
                }

                #endregion

                #region ' Close Welcome Screen! '

                if (BTC.ClickID == 239)
                {
                    DeleteBTN(232, BTC.UCID);
                    DeleteBTN(233, BTC.UCID);
                    DeleteBTN(234, BTC.UCID);
                    DeleteBTN(235, BTC.UCID);
                    DeleteBTN(236, BTC.UCID);
                    DeleteBTN(237, BTC.UCID);
                    DeleteBTN(238, BTC.UCID);
                    DeleteBTN(239, BTC.UCID);
                }

                #endregion

                #region ' Establishments/houses '
                switch (TrackName)
                {
                    case "BL1":

                        #region ' In Store '

                        if (Conn.InStore == true)
                        {
                            switch (BTC.ClickID)
                            {
                                #region ' Raffle '
                                case 120:
                                    if (Conn.Cash >= 300)
                                    {
                                        if (Conn.TotalSale >= 0)
                                        {
                                            #region ' Raffle Accept '
                                            if (Conn.LastRaffle == 0)
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
                                                #region ' Time Warning '
                                                if (Conn.LastRaffle > 120)
                                                {
                                                    MsgPly("^9 You have to wait ^1Three (3) hours ^7to rejoin the Raffle", BTC.UCID);
                                                }
                                                else if (Conn.LastRaffle > 60)
                                                {
                                                    MsgPly("^9 You have to wait ^1Two (2) hours ^7to rejoin the Raffle", BTC.UCID);
                                                }
                                                else
                                                {
                                                    MsgPly("^9 You have to wait ^1" + Conn.LastRaffle + " Minutes ^7to rejoin the Raffle", BTC.UCID);
                                                }
                                                #endregion
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            MsgPly("^9 You need to buy items before you raffle!", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^9 Not Enough Cash to join this raffle", BTC.UCID);
                                    }
                                    break;
                                #endregion

                                #region ' Jobs '
                                case 121:
                                    if (Conn.InGame == 0)
                                    {
                                        MsgPly("^9 You must be in vehicle before you access this command!", BTC.UCID);
                                    }
                                    else if (Conn.IsOfficer == true || Conn.IsCadet == true || Conn.IsTowTruck == true)
                                    {
                                        MsgPly("^9 You can only do a Job whilst not in duty!", BTC.UCID);
                                    }
                                    else if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true || Conn.InStore == true || Conn.InShop == true)
                                    {
                                        if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                                        {
                                            MsgPly("^9 You can only do 1 Job at a time!", BTC.UCID);
                                        }
                                        else if (Car == "UFR" || Car == "XFR" || Car == "FXR" || Car == "XRR" || Car == "FZR" || Car == "MRT" || Car == "FBM" || Car == "FO8" || Car == "FOX" || Car == "BF1")
                                        {
                                            MsgPly("^9 Jobs Can be only done in Road Cars!", BTC.UCID);
                                        }
                                        else if (Conn.IsSuspect == false && RobberUCID != BTC.UCID)
                                        {
                                            if (Conn.JobFromStore == false)
                                            {
                                                int JobRandom = new Random().Next(1, 45);
                                                MsgAll("^9 " + Conn.PlayerName + " started a job!");

                                                #region ' Job To Kou's '
                                                if (JobRandom > 0 && JobRandom < 15)
                                                {
                                                    if (Conn.JobToHouse1 == false)
                                                    {
                                                        if (JobRandom > 0 && JobRandom < 7)
                                                        {
                                                            MsgPly("^9 Deliver a Nintendo Wii to ^3Kou's House^7!", BTC.UCID);
                                                        }
                                                        else
                                                        {
                                                            MsgPly("^9 Deliver a Logitech G27 to ^3Kou's House^7!", BTC.UCID);
                                                        }
                                                        Conn.JobToHouse1 = true;
                                                    }
                                                }
                                                #endregion

                                                #region ' Job To Johnson's Farm '
                                                if (JobRandom > 14 && JobRandom < 30)
                                                {
                                                    if (Conn.JobToHouse2 == false)
                                                    {
                                                        if (JobRandom > 14 && JobRandom < 22)
                                                        {
                                                            MsgPly("^9 Deliver a Wooden Chair to ^3Johnson's House^7!", BTC.UCID);
                                                        }
                                                        else
                                                        {
                                                            MsgPly("^9 Deliver a Electronic Starter to ^7Johnson's House^7!", BTC.UCID);
                                                        }
                                                        Conn.JobToHouse2 = true;
                                                    }
                                                }
                                                #endregion

                                                #region ' Job To Shanen '
                                                if (JobRandom > 29)
                                                {
                                                    if (Conn.JobToHouse3 == false)
                                                    {
                                                        if (JobRandom > 29 && JobRandom < 34)
                                                        {
                                                            MsgPly("^9 Deliver a Telescope to ^3Shanen's House^7!", BTC.UCID);
                                                        }
                                                        else
                                                        {
                                                            MsgPly("^9 Deliver a PSP Games to ^3Shanen's House^7!", BTC.UCID);
                                                        }
                                                        Conn.JobToHouse3 = true;
                                                    }
                                                }
                                                #endregion

                                                #region ' Interface if Activate '
                                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                                {
                                                    DeleteBTN(121, Conn.UniqueID);
                                                    InSim.Send_BTN_CreateButton("^2You ^7can only do 1 Job at a time", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                }
                                                #endregion

                                                MsgPly("^9 You can cancel the job by typing ^2!canceljob", Conn.UniqueID);
                                                Conn.JobFromStore = true;
                                            }
                                        }
                                        else
                                        {
                                            MsgPly("^9 Can't start a Job whilst being chased.", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^9 Only in Establishments and House can start a job!", BTC.UCID);
                                    }
                                    break;
                                #endregion
                            }
                        }

                        #endregion

                        #region ' In Shop '

                        if (Conn.InShop == true)
                        {
                            switch (BTC.ClickID)
                            {
                                #region ' Fried Chicken '
                                case 118:
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
                                            MsgPly("^9 Too much health. Can't buy anymore.", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^9 Not Enought cash.", BTC.UCID);
                                    }
                                    break;
                                #endregion

                                #region ' Beer '
                                case 119:
                                    if (Conn.Cash > 10)
                                    {
                                        if (Conn.TotalHealth < 92)
                                        {
                                            MsgAll("^9 " + Conn.PlayerName + " drank some Beer!");
                                            Conn.Cash -= 10;
                                            Conn.TotalHealth += 7;
                                        }
                                        else
                                        {
                                            MsgPly("^9 Too much health. Can't buy anymore.", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^9 Not Enought cash.", BTC.UCID);
                                    }
                                    break;

                                #endregion

                                #region ' Donuts '

                                case 120:
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
                                            MsgPly("^9 Too much health. Can't buy anymore.", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^9 Not Enought cash.", BTC.UCID);
                                    }
                                    break;
                                #endregion

                                #region ' Job '
                                case 121:

                                    if (Conn.InGame == 0)
                                    {
                                        MsgPly("^9 You must be in vehicle before you access this command!", BTC.UCID);
                                    }
                                    else if (Conn.IsOfficer == true || Conn.IsCadet == true || Conn.IsTowTruck == true)
                                    {
                                        MsgPly("^9 You can only do a Job whilst not in duty!", BTC.UCID);
                                    }
                                    else if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true || Conn.InStore == true || Conn.InShop == true)
                                    {
                                        if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                                        {
                                            MsgPly("^9 You can only do 1 Job at a time!", BTC.UCID);
                                        }
                                        else if (Car == "UFR" || Car == "XFR" || Car == "FXR" || Car == "XRR" || Car == "FZR" || Car == "MRT" || Car == "FBM" || Car == "FO8" || Car == "FOX" || Car == "BF1")
                                        {
                                            MsgPly("^9 Jobs Can be only done in Road Cars!", BTC.UCID);
                                        }
                                        else if (Conn.IsSuspect == false && RobberUCID != BTC.UCID)
                                        {
                                            if (Conn.JobFromShop == false)
                                            {
                                                int JobRandom = new Random().Next(1, 45);
                                                MsgAll("^9 " + Conn.PlayerName + " started a job!");
                                                #region ' Job To Kou's House '
                                                if (JobRandom > 0 && JobRandom < 15)
                                                {
                                                    if (Conn.JobToHouse1 == false)
                                                    {
                                                        if (JobRandom > 0 && JobRandom < 7)
                                                        {
                                                            MsgPly("^9 Deliver a Hot Fries to ^3Kou's House^7!", BTC.UCID);
                                                        }
                                                        else
                                                        {
                                                            MsgPly("^9 Deliver a Pizza to ^3Kou's House^7!", BTC.UCID);
                                                        }
                                                        Conn.JobToHouse1 = true;
                                                    }
                                                }
                                                #endregion

                                                #region ' Job To Johnson's '
                                                if (JobRandom > 14 && JobRandom < 30)
                                                {
                                                    if (Conn.JobToHouse2 == false)
                                                    {
                                                        if (JobRandom > 14 && JobRandom < 22)
                                                        {
                                                            MsgPly("^9 Deliver a Burgers to ^3Johnson's House^7!", BTC.UCID);
                                                        }
                                                        else
                                                        {
                                                            MsgPly("^9 Deliver a Healthy Salad to ^3Johnson's House^7!", BTC.UCID);
                                                        }
                                                        Conn.JobToHouse2 = true;
                                                    }
                                                }
                                                #endregion

                                                #region ' Job To Shanen's '
                                                if (JobRandom > 29)
                                                {
                                                    if (Conn.JobToHouse3 == false)
                                                    {
                                                        if (JobRandom > 29 && JobRandom < 34)
                                                        {
                                                            MsgPly("^9 Deliver a Donuts to ^3Shanen's House^7!", BTC.UCID);
                                                        }
                                                        else
                                                        {
                                                            MsgPly("^9 Deliver a Burgers to ^3Shanen's House^7!", BTC.UCID);
                                                        }
                                                        Conn.JobToHouse3 = true;
                                                    }
                                                }
                                                #endregion

                                                #region ' Interface if Activate '
                                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                                {
                                                    DeleteBTN(121, Conn.UniqueID);
                                                    InSim.Send_BTN_CreateButton("^2You ^7can only do 1 Job at a time", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                }
                                                #endregion

                                                MsgPly("^9 You can cancel the job by typing ^2!canceljob", Conn.UniqueID);
                                                Conn.JobFromShop = true;
                                            }
                                        }
                                        else
                                        {
                                            MsgPly("^9 Can't start a Job whilst being chased.", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^9 Only in Establishments and House can start a job!", BTC.UCID);
                                    }
                                    break;
                                #endregion
                            }
                        }

                        #endregion

                        #region ' In School '

                        if (Conn.InSchool == true)
                        {
                            switch (BTC.ClickID)
                            {
                                #region ' Cake! '
                                case 118:
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
                                            MsgPly("^9 Too much health. Can't buy anymore.", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^9 Not Enought cash.", BTC.UCID);
                                    }
                                    break;
                                #endregion

                                #region ' Lemonade '
                                case 119:
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
                                            MsgPly("^9 Too much health. Can't buy anymore.", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^9 Not Enought cash.", BTC.UCID);
                                    }
                                    break;
                                #endregion
                            }
                        }

                        #endregion

                        #region ' InHouses '
                        if ((Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true) && BTC.ClickID == 120)
                        {
                            if (Conn.InGame == 0)
                            {
                                MsgPly("^9 You must be in vehicle before you access this command!", BTC.UCID);
                            }
                            else if (Conn.IsOfficer == true || Conn.IsCadet == true || Conn.IsTowTruck == true)
                            {
                                MsgPly("^9 You can only do a Job whilst not in duty!", BTC.UCID);
                            }
                            else if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true || Conn.InStore == true || Conn.InShop == true)
                            {
                                if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                                {
                                    MsgPly("^9 You can only do 1 Job at a time!", BTC.UCID);
                                }
                                else if (Car == "UFR" || Car == "XFR" || Car == "FXR" || Car == "XRR" || Car == "FZR" || Car == "MRT" || Car == "FBM" || Car == "FO8" || Car == "FOX" || Car == "BF1")
                                {
                                    MsgPly("^9 Jobs Can be only done in Road Cars!", BTC.UCID);
                                }
                                else if (Conn.IsSuspect == false && RobberUCID != BTC.UCID)
                                {
                                    if (Conn.JobToSchool == false)
                                    {
                                        #region ' Clear Display '

                                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                        {

                                            DeleteBTN(110, Conn.UniqueID);
                                            DeleteBTN(111, Conn.UniqueID);
                                            DeleteBTN(112, Conn.UniqueID);
                                            DeleteBTN(113, Conn.UniqueID);
                                            DeleteBTN(114, Conn.UniqueID);
                                            DeleteBTN(115, Conn.UniqueID);
                                            DeleteBTN(116, Conn.UniqueID);
                                            DeleteBTN(118, Conn.UniqueID);
                                            DeleteBTN(119, Conn.UniqueID);
                                            DeleteBTN(120, Conn.UniqueID);
                                            Conn.DisplaysOpen = false;

                                        }

                                        #endregion

                                        MsgAll("^9 " + Conn.PlayerName + " started a job!");

                                        #region ' Kou-Chan's House '
                                        if (Conn.InHouse1 == true)
                                        {
                                            if (Conn.JobFromHouse1 == false)
                                            {
                                                MsgPly("^9 Escort ^3Kou's ^7children in ^3KinderGarten ^7Safely!", Conn.UniqueID);
                                                Conn.JobFromHouse1 = true;
                                            }
                                        }
                                        #endregion

                                        #region ' Johnson's Farm '
                                        if (Conn.InHouse2 == true)
                                        {
                                            if (Conn.JobFromHouse2 == false)
                                            {
                                                MsgPly("^9 Escort ^3Johnson's ^7children in ^3KinderGarten ^7Safely!", Conn.UniqueID);
                                                Conn.JobFromHouse2 = true;
                                            }
                                        }
                                        #endregion

                                        #region ' Shanen's House '
                                        if (Conn.InHouse3 == true)
                                        {
                                            if (Conn.JobFromHouse3 == false)
                                            {
                                                MsgPly("^9 Escort ^3Shanen's ^7children in ^3KinderGarten ^7Safely!", Conn.UniqueID);
                                                Conn.JobFromHouse3 = true;
                                            }
                                        }
                                        #endregion

                                        MsgPly("^9 You can cancel the job by typing ^2!canceljob", Conn.UniqueID);
                                        Conn.JobToSchool = true;
                                    }
                                }
                                else
                                {
                                    MsgPly("^9 Can't start a Job whilst being chased.", BTC.UCID);
                                }
                            }
                            else
                            {
                                MsgPly("^9 Only in Establishments and House can start a job!", BTC.UCID);
                            }
                        }
                        #endregion

                        break;
                }
                #endregion

                #region ' Close Km info Screen! '

                if (BTC.ClickID == 121)
                {
                    DeleteBTN(101, BTC.UCID);
                    DeleteBTN(102, BTC.UCID);
                    DeleteBTN(103, BTC.UCID);
                    DeleteBTN(104, BTC.UCID);
                    DeleteBTN(105, BTC.UCID);
                    DeleteBTN(106, BTC.UCID);
                    DeleteBTN(107, BTC.UCID);
                    DeleteBTN(108, BTC.UCID);
                    DeleteBTN(109, BTC.UCID);
                    DeleteBTN(110, BTC.UCID);
                    DeleteBTN(111, BTC.UCID);
                    DeleteBTN(112, BTC.UCID);
                    DeleteBTN(113, BTC.UCID);
                    DeleteBTN(114, BTC.UCID);
                    DeleteBTN(115, BTC.UCID);
                    DeleteBTN(116, BTC.UCID);
                    DeleteBTN(117, BTC.UCID);
                    DeleteBTN(118, BTC.UCID);
                    DeleteBTN(119, BTC.UCID);
                    DeleteBTN(121, BTC.UCID);

                }
                #endregion

                #region ' Help '
                switch (BTC.ClickID)
                {
                    case 25:
                        {
                            

                            if (Conn.InGame == 0)
                            {
                                MsgPly("^9 You must be in vehicle before you access this command!", BTC.UCID);
                            }
                            else
                            {
                                if (Conn.Location.Contains("Service Station"))
                                {
                                    MsgPly("^9 You can't call a tow request in Service Station!", BTC.UCID);
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
                                                            MsgPly("^9 Please wait till your request reached!", BTC.UCID);
                                                            Conn.CalledRequest = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MsgPly("^9 There are no Tow Trucks online :(", BTC.UCID);
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    MsgPly("^9 You have called a Tow Request Please wait.", BTC.UCID);
                                                }
                                            }
                                            else
                                            {
                                                MsgPly("^9 You can't call a request whilst your vehicle is running!", BTC.UCID);
                                            }
                                        }
                                        else
                                        {
                                            MsgPly("^9 Your already being towed!", BTC.UCID);

                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^9 You can't call a request whilst being duty!", BTC.UCID);
                                    }
                                }
                            } 
                           
                        }
                        break;
                }
                #endregion

                #region ' Cop Panel '

                if (Conn.IsOfficer == true || Conn.IsCadet == true)
                {
                    if (Conn.CopPanel == 1)
                    {
                        #region ' Engage '
                        if (Conn.InChaseProgress == false)
                        {
                            switch (BTC.ClickID)
                            {
                                #region ' Engage! '
                                case 20:
                                    try
                                    {
                                        if (Conn.UniqueID == BTC.UCID)
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
                                                            MsgPly("^9 Maximum Pursuit Limit: ^7" + AddChaseLimit, BTC.UCID);
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
                                                                MsgPly("^9 " + Connections[ChaseeIndex].PlayerName + " is being busted a cop.", BTC.UCID);
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
                                                        MsgPly("^9 Cannot join in the Police Pursuit.", BTC.UCID);
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    MsgPly("^9 No Civilian found in 150 meters", BTC.UCID);
                                                }
                                            }

                                            #endregion
                                        }
                                    }
                                    catch
                                    {
                                        MsgPly("^9 Engage Error.", BTC.UCID);
                                    }
                                    break;
                                #endregion
                            }
                        }
                        #endregion

                        #region ' In Chase Progress '
                        else
                        {
                            switch (BTC.ClickID)
                            {
                                #region ' Bump or Busted '
                                case 20:

                                    #region ' Busted '
                                    if (Conn.Busted == true)
                                    {
                                        if (Conn.BustedTimer == 5)
                                        {
                                            #region ' Close Moderation if Possible '
                                            if (Conn.InModerationMenu == 1 || Conn.InModerationMenu == 2)
                                            {
                                                DeleteBTN(30, BTC.UCID);
                                                DeleteBTN(31, BTC.UCID);
                                                DeleteBTN(32, BTC.UCID);
                                                DeleteBTN(33, BTC.UCID);
                                                DeleteBTN(34, BTC.UCID);
                                                DeleteBTN(35, BTC.UCID);
                                                DeleteBTN(36, BTC.UCID);
                                                DeleteBTN(37, BTC.UCID);
                                                DeleteBTN(38, BTC.UCID);
                                                DeleteBTN(39, BTC.UCID);
                                                DeleteBTN(40, BTC.UCID);
                                                DeleteBTN(41, BTC.UCID);
                                                DeleteBTN(42, BTC.UCID);
                                                DeleteBTN(43, BTC.UCID);
                                                Conn.ModReason = "";
                                                Conn.ModReasonSet = false;
                                                Conn.ModUsername = "";
                                                Conn.InModerationMenu = 0;
                                            }
                                            #endregion

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

                                    #region ' Chase Bump '
                                    else if (Conn.Busted == false)
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
                                        MsgPly("^9 type ^2!busted ^7to busted the suspect", BTC.UCID);
                                        MsgPly("^7  or ^2!disengage ^7to stop the chase!", BTC.UCID);
                                    }

                                    #endregion

                                    break;

                                #endregion

                                #region ' Disengage '
                                case 21:
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
                                                MsgPly("^9 You aren't in chase!", BTC.UCID);
                                            }
                                        }
                                        else if (Conn.InGame == 0)
                                        {
                                            MsgPly("^9 You must be in vehicle before you access this command!", BTC.UCID);
                                        }
                                    }
                                    else if (Conn.InFineMenu == true)
                                    {
                                        MsgPly("^9 Set a Tickets to " + ChaseCon.PlayerName + "!", BTC.UCID);
                                    }

                                    break;
                                #endregion
                            }
                        }
                        #endregion

                        #region ' Remove Trap '

                        if (Conn.TrapSetted == true && BTC.ClickID == 21)
                        {
                            MsgPly("^9 Speed Trap Removed", Conn.UniqueID);
                            Conn.TrapY = 0;
                            Conn.TrapX = 0;
                            Conn.TrapSpeed = 0;
                            Conn.TrapSetted = false;
                        }

                        #endregion
                    }
                }

                #endregion

                #region ' Busted Panel & Accept/Refuse Issue '

                if (Conn.InFineMenu == true)
                {
                    switch (BTC.ClickID)
                    {
                        #region ' Warn '
                        case 39:

                            if (Conn.TicketReasonSet == true)
                            {
                                MsgAll("^9 " + Conn.PlayerName + " issued a warn to " + ChaseCon.PlayerName);

                                #region ' To Connection List '
                                foreach (clsConnection Con in Connections)
                                {
                                    if (Con.UniqueID == ChaseCon.UniqueID)
                                    {
                                        InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 30, (Con.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 31, (Con.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Ticket Window", Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 32, (Con.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Issued by: " + Conn.PlayerName, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 60, 54, 33, (Con.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Chase Condition: " + Conn.ChaseCondition, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 65, 54, 34, (Con.UniqueID), 2, false);

                                        InSim.Send_BTN_CreateButton("^7Fine: None ", Flags.ButtonStyles.ISB_LIGHT, 5, 45, 78, 77, 36, (Con.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Reason: " + Conn.TicketReason, Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 37, (Con.UniqueID), 2, false);

                                        InSim.Send_BTN_CreateButton("^7You are only warned from this Ticket!", Flags.ButtonStyles.ISB_C1, 4, 70, 95, 65, 38, (Con.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^1^J‚w", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 6, 52, 143, 39, (Con.UniqueID), 40, false);
                                        Con.AcceptTicket = 2;
                                        Con.IsBeingBusted = false;
                                        Con.TicketRefuse = 0;
                                    }
                                }
                                #endregion

                                #region ' Close Region LOL '
                                DeleteBTN(30, BTC.UCID);
                                DeleteBTN(31, BTC.UCID);
                                DeleteBTN(32, BTC.UCID);
                                DeleteBTN(33, BTC.UCID);
                                DeleteBTN(34, BTC.UCID);
                                DeleteBTN(35, BTC.UCID);
                                DeleteBTN(36, BTC.UCID);
                                DeleteBTN(37, BTC.UCID);
                                DeleteBTN(38, BTC.UCID);
                                DeleteBTN(39, BTC.UCID);
                                DeleteBTN(40, BTC.UCID);
                                #endregion

                                if (Conn.JoinedChase == true)
                                {
                                    Conn.JoinedChase = false;
                                }
                                if (Conn.InFineMenu == true)
                                {
                                    Conn.InFineMenu = false;
                                }
                                Conn.TicketAmount = 0;
                                Conn.TicketAmountSet = false;
                                Conn.TicketReason = "";
                                Conn.TicketReasonSet = false;
                                Conn.Busted = false;
                                Conn.Chasee = -1;
                                Conn.ChaseCondition = 0;
                            }
                            else if (Conn.TicketReasonSet == false)
                            {
                                MsgPly("^9 Warn Error. You must have Reason.", Conn.UniqueID);
                            }

                            break;
                        #endregion

                        #region ' Issue '
                        case 40:

                            if (Conn.TicketReasonSet == true && Conn.TicketAmountSet == true)
                            {
                                MsgAll("^9 " + Conn.PlayerName + " issued a fines to " + ChaseCon.PlayerName);

                                #region ' To Connection List '
                                foreach (clsConnection Con in Connections)
                                {
                                    if (Con.UniqueID == ChaseCon.UniqueID)
                                    {
                                        if (Con.TicketRefuse == 0)
                                        {
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 30, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 31, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Ticket Window", Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 32, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Issued by: " + Conn.PlayerName, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 60, 54, 33, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Chase Condition: " + Conn.ChaseCondition, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 65, 54, 34, (Con.UniqueID), 2, false);

                                            InSim.Send_BTN_CreateButton("^7Fine ^1$" + Conn.TicketAmount, Flags.ButtonStyles.ISB_LIGHT, 5, 45, 78, 77, 36, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Reason: " + Conn.TicketReason, Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 37, (Con.UniqueID), 2, false);

                                            InSim.Send_BTN_CreateButton("^7This Ticket is being issued for being Busted!", Flags.ButtonStyles.ISB_C1, 4, 70, 95, 65, 38, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Refuse", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 4, 10, 103, 107, 39, (Con.UniqueID), 40, false);
                                            InSim.Send_BTN_CreateButton("^7Pay", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 4, 10, 103, 82, 40, (Con.UniqueID), 40, false);
                                            Con.AcceptTicket = 1;
                                            Con.TicketRefuse = 1;
                                            Con.TicketReason = Conn.TicketReason;
                                            Con.TicketAmount = Conn.TicketAmount;
                                        }
                                        else if (Con.TicketRefuse == 1)
                                        {
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 30, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 31, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Ticket Window", Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 32, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Issued by: " + Conn.PlayerName, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 60, 54, 33, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Chase Condition: " + Conn.ChaseCondition, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 65, 54, 34, (Con.UniqueID), 2, false);

                                            InSim.Send_BTN_CreateButton("^7Fine ^1$" + Conn.TicketAmount, Flags.ButtonStyles.ISB_LIGHT, 5, 45, 78, 77, 36, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Reason: " + Conn.TicketReason, Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 37, (Con.UniqueID), 2, false);

                                            InSim.Send_BTN_CreateButton("^7This is the last Ticket Issue. Remember you'll get fined in the next Refuse!", Flags.ButtonStyles.ISB_C1, 4, 70, 95, 65, 38, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Refuse", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 4, 10, 103, 107, 39, (Con.UniqueID), 40, false);
                                            InSim.Send_BTN_CreateButton("^7Pay", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 4, 10, 103, 82, 40, (Con.UniqueID), 40, false);
                                            Con.AcceptTicket = 1;
                                            Con.TicketRefuse = 2;
                                            Con.TicketReason = Conn.TicketReason;
                                            Con.TicketAmount = Conn.TicketAmount;
                                        }
                                    }
                                    if (Con.Chasee == ChaseCon.UniqueID)
                                    {
                                        if (ChaseCon.CopInChase > 1)
                                        {
                                            Con.TicketAmount = Conn.TicketAmount;
                                            Con.TicketReason = Conn.TicketReason;
                                        }
                                    }
                                }
                                #endregion

                                #region ' Close Region LOL '
                                DeleteBTN(30, BTC.UCID);
                                DeleteBTN(31, BTC.UCID);
                                DeleteBTN(32, BTC.UCID);
                                DeleteBTN(33, BTC.UCID);
                                DeleteBTN(34, BTC.UCID);
                                DeleteBTN(35, BTC.UCID);
                                DeleteBTN(36, BTC.UCID);
                                DeleteBTN(37, BTC.UCID);
                                DeleteBTN(38, BTC.UCID);
                                DeleteBTN(39, BTC.UCID);
                                DeleteBTN(40, BTC.UCID);
                                #endregion
                            }
                            else if (Conn.TicketAmountSet == false && Conn.TicketReasonSet == false)
                            {
                                MsgPly("^9 Issue Error. You must have Reason and Ticket Fines.", Conn.UniqueID);
                            }

                            break;
                        #endregion
                    }
                }

                #region ' Pay/Warn/Refuse '

                #region ' Pay/Refuse '
                if (Conn.AcceptTicket == 1)
                {
                    switch (BTC.ClickID)
                    {
                        #region ' Refuse '
                        case 39:

                            if (Conn.TicketRefuse == 1)
                            {
                                MsgAll("^9 " + Conn.PlayerName + " refused to pay the first ticket!");
                                MsgPly("^9 WARNING: ^7Refusing the second ticket may cause max fine!", BTC.UCID);
                                #region ' To Connection List '

                                foreach (clsConnection o in Connections)
                                {
                                    if (o.Chasee == Conn.UniqueID)
                                    {
                                        if (o.InFineMenu == true)
                                        {
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 30, (o.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 31, (o.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Ticket Window Re-Issue", Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 32, (o.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Suspect Name: " + Conn.PlayerName, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 60, 54, 33, (o.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Chase Condition: " + o.ChaseCondition, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 65, 54, 34, (o.UniqueID), 2, false);

                                            #region ' Condition '
                                            if (Conn.ChaseCondition == 1)
                                            {
                                                InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$500", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 54, 35, (o.UniqueID), 2, false);
                                            }
                                            if (Conn.ChaseCondition == 2)
                                            {
                                                InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$1,300", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 58, 35, (o.UniqueID), 2, false);
                                            }
                                            if (Conn.ChaseCondition == 3)
                                            {
                                                InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$2,500", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 58, 35, (o.UniqueID), 2, false);
                                            }
                                            if (Conn.ChaseCondition == 4)
                                            {
                                                InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$3,500", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 58, 35, (o.UniqueID), 2, false);
                                            }
                                            if (Conn.ChaseCondition == 5)
                                            {
                                                InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$5,000", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 58, 35, (o.UniqueID), 2, false);
                                            }
                                            #endregion

                                            // Click Buttons
                                            InSim.Send_BTN_CreateButton("^7Reason", "Enter the chase reason", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 78, 77, 64, 36, (o.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (o.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Enter a Fine Amount And Reason For The Chase", Flags.ButtonStyles.ISB_C1, 4, 70, 95, 65, 38, (o.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Warn", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 4, 10, 103, 107, 39, (o.UniqueID), 40, false);
                                            InSim.Send_BTN_CreateButton("^7Issue", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 4, 10, 103, 82, 40, (o.UniqueID), 40, false);

                                        }
                                    }
                                }
                                #endregion

                                Conn.TicketReason = "";
                                Conn.TicketAmount = 0;
                                Conn.AcceptTicket = 0;

                                #region ' Close Region LOL '
                                DeleteBTN(30, BTC.UCID);
                                DeleteBTN(31, BTC.UCID);
                                DeleteBTN(32, BTC.UCID);
                                DeleteBTN(33, BTC.UCID);
                                DeleteBTN(34, BTC.UCID);
                                DeleteBTN(35, BTC.UCID);
                                DeleteBTN(36, BTC.UCID);
                                DeleteBTN(37, BTC.UCID);
                                DeleteBTN(38, BTC.UCID);
                                DeleteBTN(39, BTC.UCID);
                                DeleteBTN(40, BTC.UCID);
                                #endregion
                            }
                            else if (Conn.TicketRefuse == 2)
                            {
                                MsgAll("^9 " + Conn.PlayerName + " refused to pay the fines!");
                                MsgAll("  ^7was fined ^1$5000 ^7for refusing second ticket!");

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
                                DeleteBTN(30, BTC.UCID);
                                DeleteBTN(31, BTC.UCID);
                                DeleteBTN(32, BTC.UCID);
                                DeleteBTN(33, BTC.UCID);
                                DeleteBTN(34, BTC.UCID);
                                DeleteBTN(35, BTC.UCID);
                                DeleteBTN(36, BTC.UCID);
                                DeleteBTN(37, BTC.UCID);
                                DeleteBTN(38, BTC.UCID);
                                DeleteBTN(39, BTC.UCID);
                                DeleteBTN(40, BTC.UCID);
                                #endregion
                            }

                            break;
                        #endregion

                        #region ' Pay '
                        case 40:

                            MsgAll("^9 " + Conn.PlayerName + " paid the fine for ^1$" + Conn.TicketAmount + "^7!");
                            MsgAll("^9 Reason: " + Conn.TicketReason);


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
                                        MsgAll("^9 " + i.PlayerName + " was rewarded for ^2$" + (Convert.ToInt16(i.TicketAmount * 0.4)));
                                        i.Cash += (Convert.ToInt16(i.TicketAmount * 0.4));
                                    }
                                    if (i.IsCadet == true)
                                    {
                                        MsgAll("^9 " + i.PlayerName + " was rewarded for ^2$" + (Convert.ToInt16(i.TicketAmount * 0.2)));
                                        i.Cash += (Convert.ToInt16(i.TicketAmount * 0.2));
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

                            Conn.Cash -= Conn.TicketAmount;

                            Conn.TicketReason = "";
                            Conn.TicketAmount = 0;
                            Conn.TicketRefuse = 0;
                            Conn.AcceptTicket = 0;
                            Conn.IsBeingBusted = false;
                            Conn.CopInChase = 0;

                            #region ' Close Region LOL '
                            DeleteBTN(30, BTC.UCID);
                            DeleteBTN(31, BTC.UCID);
                            DeleteBTN(32, BTC.UCID);
                            DeleteBTN(33, BTC.UCID);
                            DeleteBTN(34, BTC.UCID);
                            DeleteBTN(35, BTC.UCID);
                            DeleteBTN(36, BTC.UCID);
                            DeleteBTN(37, BTC.UCID);
                            DeleteBTN(38, BTC.UCID);
                            DeleteBTN(39, BTC.UCID);
                            DeleteBTN(40, BTC.UCID);
                            #endregion

                            break;
                        #endregion
                    }
                }
                #endregion

                #region ' Warn '
                else if (Conn.AcceptTicket == 2)
                {
                    #region ' Close Warn '
                    if (BTC.ClickID == 39)
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
                                MsgAll("^9 " + Conn.PlayerName + " accepted the Warning.");
                                MsgAll("^9 Reason: " + i.TicketReason);
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

                        #region ' Close Region LOL '
                        DeleteBTN(30, BTC.UCID);
                        DeleteBTN(31, BTC.UCID);
                        DeleteBTN(32, BTC.UCID);
                        DeleteBTN(33, BTC.UCID);
                        DeleteBTN(34, BTC.UCID);
                        DeleteBTN(35, BTC.UCID);
                        DeleteBTN(36, BTC.UCID);
                        DeleteBTN(37, BTC.UCID);
                        DeleteBTN(38, BTC.UCID);
                        DeleteBTN(39, BTC.UCID);
                        DeleteBTN(40, BTC.UCID);
                        #endregion
                    }
                    #endregion
                }
                #endregion

                #endregion

                #endregion

                #region ' Moderation Panel '

                #region ' Close Moderation '
                if (BTC.ClickID == 43)
                {
                    if (Conn.InModerationMenu == 1 || Conn.InModerationMenu == 2)
                    {
                        DeleteBTN(30, BTC.UCID);
                        DeleteBTN(31, BTC.UCID);
                        DeleteBTN(32, BTC.UCID);
                        DeleteBTN(33, BTC.UCID);
                        DeleteBTN(34, BTC.UCID);
                        DeleteBTN(35, BTC.UCID);
                        DeleteBTN(36, BTC.UCID);
                        DeleteBTN(37, BTC.UCID);
                        DeleteBTN(38, BTC.UCID);
                        DeleteBTN(39, BTC.UCID);
                        DeleteBTN(40, BTC.UCID);
                        DeleteBTN(41, BTC.UCID);
                        DeleteBTN(42, BTC.UCID);
                        DeleteBTN(43, BTC.UCID);
                        Conn.ModReason = "";
                        Conn.ModReasonSet = false;
                        Conn.ModUsername = "";
                        Conn.InModerationMenu = 0;
                    }
                }

                #endregion

                if (Conn.InModerationMenu == 1)
                {

                    switch (BTC.ClickID)
                    {
                        #region ' Warn '
                        case 38:

                            if (Conn.ModReasonSet == true)
                            {
                                bool Found = false;
                                bool Complete = false;
                                #region ' Online '
                                foreach (clsConnection i in Connections)
                                {
                                    if (i.Username == Conn.ModUsername)
                                    {
                                        Found = true;

                                        if (i.OnScreenExit > 0)
                                        {
                                            MsgPly("^9 Please wait until" + i.PlayerName + " completes the exit screen!", BTC.UCID);
                                        }
                                        else
                                        {
                                            Complete = true;
                                            InSim.Send_BTN_CreateButton(Conn.PlayerName + "^7#(W) : " + Conn.ModReason, Flags.ButtonStyles.ISB_C2, 14, 200, 50, 0, 10, i.UniqueID, 2, false);
                                            MsgPly("^9 You are warned by " + Conn.PlayerName, i.UniqueID);
                                            
                                            i.ModerationWarn = 5;
                                        }
                                    }
                                }
                                #endregion

                                #region ' Found '

                                if (Found == true && Complete == true)
                                {
                                    InSim.Send_BTN_CreateButton("^4>> ^7Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 69, 82, 54, 52, 37, BTC.UCID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8FINE", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 68, 39, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8BAN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 110, 42, BTC.UCID, 2, false);

                                    Conn.ModReasonSet = false;
                                    Conn.ModReason = "";
                                }

                                #endregion

                                #region ' Offline '
                                if (Found == false)
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

                                    MsgPly("^9 " + PlayerName + " couldn't be warned.", BTC.UCID);
                                    MsgPly("^9 The user goes offline mode!", BTC.UCID);

                                    InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTC.UCID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                    Conn.ModerationWarn = 2;
                                    Conn.ModReasonSet = false;
                                    Conn.ModReason = "";
                                }
                                #endregion
                            }
                            else
                            {
                                MsgPly("^9 Reason not yet setted.", BTC.UCID);
                            }

                            break;
                        #endregion

                        #region ' Spectate '
                        case 40:

                            if (Conn.ModReasonSet == true)
                            {
                                bool Found = false;
                                bool Complete = false;
                                #region ' Online '
                                foreach (clsConnection i in Connections)
                                {
                                    if (i.Username == Conn.ModUsername)
                                    {
                                        Found = true;

                                        if (i.InGame == 0)
                                        {
                                            MsgPly("^9 " + i.PlayerName + " is not ingame!", BTC.UCID);
                                        }
                                        else
                                        {
                                            Complete = true;
                                            MsgAll("^9 " + i.PlayerName + " was spected by " + Conn.PlayerName + "!");
                                            MsgPly("^9 You are spected by " + Conn.PlayerName, i.UniqueID);
                                            MsgAll("^9 Reason: " + Conn.ModReason);
                                            
                                            SpecID(i.Username);
                                            SpecID(i.PlayerName);
                                        }

                                    }
                                }
                                #endregion

                                #region ' Found '

                                if (Found == true && Complete == true)
                                {
                                    InSim.Send_BTN_CreateButton("^4>> ^7Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 69, 82, 54, 52, 37, BTC.UCID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8FINE", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 68, 39, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8BAN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 110, 42, BTC.UCID, 2, false);

                                    Conn.ModReasonSet = false;
                                    Conn.ModReason = "";
                                }

                                #endregion

                                #region ' Goes Offline '
                                if (Found == false)
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

                                    MsgPly("^9 " + PlayerName + " couldn't be spectated.", BTC.UCID);
                                    MsgPly("^9 The user goes offline mode!", BTC.UCID);

                                    InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTC.UCID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                    Conn.ModerationWarn = 2;
                                    Conn.ModReasonSet = false;
                                    Conn.ModReason = "";
                                }
                                #endregion
                            }
                            else
                            {
                                MsgPly("^9 Reason not yet setted.", BTC.UCID);
                            }

                            break;
                        #endregion

                        #region ' Kick '
                        case 41:

                            if (Conn.ModReasonSet == true)
                            {
                                bool Found = false;

                                #region ' Online '
                                foreach (clsConnection i in Connections)
                                {
                                    if (i.Username == Conn.ModUsername)
                                    {
                                        Found = true;
                                        MsgAll("^9 " + i.PlayerName + " was kicked by  " + Conn.PlayerName + "!");
                                        MsgPly("^9 You are kicked by " + Conn.PlayerName, i.UniqueID);
                                        MsgAll("^9 Reason: " + Conn.ModReason);
                                        
                                        KickID(i.Username);
                                    }
                                }
                                #endregion

                                #region ' Found '

                                if (Found == true)
                                {
                                    InSim.Send_BTN_CreateButton("^4>> ^7Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 69, 82, 54, 52, 37, BTC.UCID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8FINE", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 68, 39, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8BAN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 110, 42, BTC.UCID, 2, false);

                                    Conn.ModReasonSet = false;
                                    Conn.ModReason = "";
                                }

                                #endregion

                                #region ' Offline '
                                if (Found == false)
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

                                    MsgPly("^9 " + PlayerName + " couldn't be kicked.", BTC.UCID);
                                    MsgPly("^9 The user goes offline mode!", BTC.UCID);

                                    InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTC.UCID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                    Conn.ModerationWarn = 2;
                                    Conn.ModReasonSet = false;
                                    Conn.ModReason = "";
                                }
                                #endregion
                            }
                            else
                            {
                                MsgPly("^9 Reason not yet setted.", BTC.UCID);
                            }

                            break;
                        #endregion
                    }

                }

                #endregion

            }
            catch { }
        }
    }
}