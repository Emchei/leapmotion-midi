/******************************************************************************\
* Sample Leap. April 2016                                                     *
* E.K. Yeboah                                                                 *
* input Leap motion and the output the tracking data on the GUILeapDemo form  *
\******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO.Ports;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;
using Sanford.Multimedia.Timers;
using Leap;


namespace sample_leap
{



    public partial class GUILeapDemo : Form, ILeapEventDelegate
    {
        private Controller controller;
        private LeapEventListener listener;


        private OutputDevice outDevice;
        private int outDeviceID = 1;
        private Map_Value limit;
        private Serial__communication serial_com;



        public GUILeapDemo()
        {
            InitializeComponent();

            this.controller = new Controller();
            this.listener = new LeapEventListener(this);
            controller.AddListener(listener);
            limit = new Map_Value();
            serial_com = new Serial__communication();
        }



        protected override void OnLoad(EventArgs e)
        {
            if (!controller.IsConnected)
            {
                MessageBox.Show("No Leap motion device plugged in", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();
            }
            else
            {


                if (OutputDevice.DeviceCount == 0)
                {
                    MessageBox.Show("There is output MIDI device.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Close();
                }
                else
                {
                    try
                    {
                        this.outDevice = new OutputDevice(outDeviceID);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.AbortRetryIgnore);
                        Close();
                    }
                }
            }
            base.OnLoad(e);
        }


        public void LeapEventNotification(string EventName)
        {


            if (!this.InvokeRequired)
            {
                switch (EventName)
                {
                    case "onInit":
                        Debug.WriteLine("Init");
                        break;
                    case "onConnect":
                        //this.connectHandler();
                        break;
                    case "onFrame":
                        if (!this.Disposing)
                            this.FrameControlHandler(this.controller.Frame());
                        break;
                }
            }
            else
            {
                BeginInvoke(new LeapEventDelegate(LeapEventNotification), new object[] { EventName });
            }
        }




        void FrameControlHandler(Frame frame)
        {
            //************************************************ GRAB *************************************************************//
            float GrabstrengthFirstHand = frame.Hands[0].GrabStrength;
            float GrabstrengthSecondHand = frame.Hands[1].GrabStrength;



            decimal _grabOne = limit.MapValue((decimal)GrabstrengthFirstHand, 0, 1, 0, 127);
            decimal _grabTwo = limit.MapValue((decimal)GrabstrengthSecondHand, 0, 1, 0, 127);

            int _grabstrengthOne = (int)Math.Round(_grabOne);
            int _grabstrengthTwo = (int)Math.Round(_grabTwo);

            //************************************************ end of GRAB *************************************************************//
            //************************************************ FIRST HAND FEATURES *************************************************************//
            decimal FH_yAxisPointing = (decimal)frame.Hands[0].PalmPosition.y;
            decimal FH_xAxisPointing = (decimal)frame.Hands[0].PalmPosition.x;
            decimal FH_zAxisPointing = (decimal)frame.Hands[0].PalmPosition.z;


            decimal FH_handspeed_ = (decimal)frame.Hands[0].PalmVelocity.Magnitude;//the instantaneous speed of the hand object.

            decimal FH_yaxis = limit.MapValue(FH_yAxisPointing, 0, 450, 0, 127);
            decimal FH_xaxis = limit.MapValue(FH_xAxisPointing, -150, 150, 0, 127);
            decimal FH_zaxis = limit.MapValue(FH_zAxisPointing, -150, 150, 0, 127);
            decimal FH_speed = limit.MapValue(FH_handspeed_, 0, 1000, 0, 127);

            int FH_realtime_yaxis = (int)Math.Round(FH_yaxis);
            int FH_realtime_xaxis = (int)Math.Round(FH_xaxis);
            int FH_realtime_zaxis = (int)Math.Round(FH_zaxis);
            int FH_realtimeSpeed = (int)Math.Round(FH_speed);
            //************************************************ end of FIRST HAND FEATURES *************************************************************//
            //************************************************ SECOND HAND FEATURES *************************************************************//
            decimal SH_yAxisPointing = (decimal)frame.Hands[1].PalmPosition.y;
            decimal SH_xAxisPointing = (decimal)frame.Hands[1].PalmPosition.x;
            decimal SH_zAxisPointing = (decimal)frame.Hands[1].PalmPosition.z;

            decimal SH_handspeed_ = (decimal)frame.Hands[1].PalmVelocity.Magnitude;




            decimal SH_yaxis = limit.MapValue(SH_yAxisPointing, 0, 450, 0, 127);
            decimal SH_xaxis = limit.MapValue(SH_xAxisPointing, -150, 150, 0, 127);
            decimal SH_zaxis = limit.MapValue(SH_zAxisPointing, -150, 150, 0, 127);
            decimal SH_speed = limit.MapValue(SH_handspeed_, 0, 1000, 0, 127);

            int SH_realtime_yaxis = (int)Math.Round(SH_yaxis);
            int SH_realtime_xaxis = (int)Math.Round(SH_xaxis);
            int SH_realtime_zaxis = (int)Math.Round(SH_zaxis);
            int SH_realtimeSpeed = (int)Math.Round(SH_speed);

            //************************************************ end of SECOND HAND FEATURES *************************************************************//
            //************************************************ palm orientation   *************************************************************//
            Vector FH_normal = frame.Hands[0].PalmNormal;
            Vector FH_direction = frame.Hands[0].Direction;

            float FH_pitch = FH_direction.Pitch * (180.0f / (float)Math.PI);
            float FH_roll = FH_normal.Roll * (180.0f / (float)Math.PI);
            float FH_yaw = FH_direction.Yaw * (180.0f / (float)Math.PI);

            decimal FH_mappedYaw = limit.MapValue((decimal)FH_yaw, -120, 120, 0, 127);
            decimal FH_mappedPitch = limit.MapValue((decimal)FH_pitch, -120, 120, 0, 127);
            decimal FH_mappedRoll = limit.MapValue((decimal)FH_roll, -120, 120, 0, 127);

            int _FHyaw = (int)Math.Round(FH_mappedYaw);
            int _FHpitch = (int)Math.Round(FH_mappedPitch);
            int _FHroll = (int)Math.Round(FH_mappedRoll);

            Vector SH_normal = frame.Hands[1].PalmNormal;
            Vector SH_direction = frame.Hands[1].Direction;

            float SH_pitch = SH_direction.Pitch * (180.0f / (float)Math.PI);
            float SH_roll = SH_normal.Roll * (180.0f / (float)Math.PI);
            float SH_yaw = SH_direction.Yaw * (180.0f / (float)Math.PI);

            decimal SH_mappedYaw = limit.MapValue((decimal)SH_yaw, -120, 120, 0, 127);
            decimal SH_mappedPitch = limit.MapValue((decimal)SH_pitch, -120, 120, 0, 127);
            decimal SH_mappedRoll = limit.MapValue((decimal)SH_roll, -120, 120, 0, 127);

            int _SHyaw = (int)Math.Round(SH_mappedYaw);
            int _SHpitch = (int)Math.Round(SH_mappedPitch);
            int _SHroll = (int)Math.Round(SH_mappedRoll);

            //************************************************ end of palm orientation   *************************************************************//

            //************************************************ finger-spread   *************************************************************//
            //FIRST
            Finger FH_indexFinger = frame.Hands[0].Fingers[(int)Finger.FingerType.TYPE_INDEX];
            Finger FH_middleFinger = frame.Hands[0].Fingers[(int)Finger.FingerType.TYPE_MIDDLE];

            Vector FH_index_fingerVec = FH_indexFinger.Direction;
            Vector FH_middle_fingerVec = FH_middleFinger.Direction;

            double FH_dotProduct = FH_indexFinger.Direction.Dot(FH_middle_fingerVec);
            double FH_angle_IM = Math.Acos(FH_dotProduct) * 180 / Math.PI;

            decimal FH_fingers = limit.MapValue((decimal)FH_angle_IM, 5, 34, 0, 127);
            int FH_Angle_fingerspread = (int)Math.Round(FH_fingers);

            //SECOND
            Finger SH_indexFinger = frame.Hands[1].Fingers[(int)Finger.FingerType.TYPE_INDEX];
            Finger SH_middleFinger = frame.Hands[1].Fingers[(int)Finger.FingerType.TYPE_MIDDLE];

            Vector SH_index_fingerVec = SH_indexFinger.Direction;
            Vector SH_middle_fingerVec = SH_middleFinger.Direction;

            double SH_dotProduct = SH_indexFinger.Direction.Dot(SH_middle_fingerVec);
            double SH_angle_IM = Math.Acos(SH_dotProduct) * 180 / Math.PI;

            decimal SH_fingers = limit.MapValue((decimal)SH_angle_IM, 5, 34, 0, 127);
            int SH_Angle_fingerspread = (int)Math.Round(SH_fingers);

            //************************************************ end of finger-spread *************************************************************//

            //********************************************************Time-visible *************************************************************//
            float presenceOfFirstHand = frame.Hands[0].TimeVisible;
            decimal FH_TimeOftrackingSec = limit.MapValue((decimal)presenceOfFirstHand, 0, 300, 0, 127);
            int FH_timeVisible = (int)Math.Round(FH_TimeOftrackingSec);

            float presenceOfSecondHand = frame.Hands[1].TimeVisible;
            decimal SH_TimeOftrackingSec = limit.MapValue((decimal)presenceOfSecondHand, 0, 300, 0, 127);
            int SH_timeVisible = (int)Math.Round(SH_TimeOftrackingSec);

            //************************************************ end of Time Visible *************************************************************// 




            if (frame.Hands.IsEmpty)
            {
                handsdection_gB.BackColor = Color.Red;

                if (ActivateHandDetection_box.Checked)
                {
                    SendControlChange(1, 3, 0);
                    handON.Text = "0";
                    second_handON.Text = "0";
                }

            }
            else
            {
                handsdection_gB.BackColor = Color.Aquamarine;

                if (ActivateHandDetection_box.Checked)
                {
                    if (frame.Hands.Count >= 1)
                    {
                        if (frame.Hands[0].IsValid)
                        {
                            SendControlChange(1, 3, 127);
                            handON.Text = "127";
                        }


                        if (frame.Hands[1].IsValid)
                        {
                            if (SH_handdetection_cB.Checked)
                            {
                                SendControlChange(1, 9, 127);
                                second_handON.Text = "127";
                            }

                        }

                    }
                    else {; }

                }

            }



            
            if (ActivatePalm_box.Checked)
            {
                

                SecondHand_MiDiCC_groupBox.Enabled = true;
                Firsthand_MiDiCC_groupBox.Enabled = true;
                UserDefinedCC_groupBox.Enabled = true;




                if (frame.Hands.Count >= 1)
                {

                    if (SH_speed_cB.Checked)
                    {
                        SendControlChange(1, 14, SH_realtimeSpeed);
                        SH_CCHandspeed.Text = SH_realtimeSpeed.ToString();
                      
                    }
                    else {; }

                    if (SH_PositionY_cB.Checked)
                    {
                       
                        SendControlChange(1, 15, SH_realtime_yaxis);
                        SH_MidiCC_YPos.Text = SH_realtime_yaxis.ToString();
                       
                       

                    }
                    else {; }

                    if (SH_PositionX_cB.Checked)
                    {
                        SendControlChange(1, 20, SH_realtime_xaxis);
                        SH_MidiCC_XPos.Text = SH_realtime_xaxis.ToString();
                       
                    }
                    else {; }

                    if (SH_PositionZ_cB.Checked)
                    {
                        SendControlChange(1, 21, SH_realtime_zaxis);
                        SH_MidiCC_ZPos.Text = SH_realtime_zaxis.ToString();

                    }
                    else {; }

                    if (SH_Yaw_cB.Checked)
                    {
                        SendControlChange(1, 22, _SHyaw);
                        SH_YawMidiCC.Text = _SHyaw.ToString();
                    }
                    else {; }

                    if (SH_Roll_cB.Checked)
                    {
                        SendControlChange(1, 23, _SHroll);
                        SH_RollMidiCC.Text = _SHroll.ToString();
                    }

                    else {; }

                    if (SH_Pitch_cB.Checked)
                    {
                        SendControlChange(1, 24, _SHpitch);
                        SH_PitchMidiCC.Text = _SHpitch.ToString();
                    }
                    else {; }

                    if (SH_fingerspread_cB.Checked)
                    {
                        SendControlChange(1, 25, SH_Angle_fingerspread);
                        SH_MidiCC_fingerSpread.Text = SH_Angle_fingerspread.ToString();
                    }

                    if (SH_timeVisible_cB.Checked)
                    {
                        SendControlChange(1, 26, SH_timeVisible);
                        SH_MidiCC_tV.Text = SH_timeVisible.ToString();
                    }
                    else {; }

                    //*****************FISRT HAND******************//
                    if (FH_speed_cB.Checked)
                    {
                        SendControlChange(1, 27, FH_realtimeSpeed);
                        FH_CCHandspeed.Text = FH_realtimeSpeed.ToString();
                    }
                    else {; }

                    if (FH_PositionY_cB.Checked)
                    {
                        
                        SendControlChange(1, 28, FH_realtime_yaxis);
                        FH_MidiCC_YPos.Text = FH_realtime_yaxis.ToString();

                    }
                    else {; }



                    if (FH_PositionX_cB.Checked)
                    {
                        SendControlChange(1, 29, FH_realtime_xaxis);
                        FH_MidiCC_XPos.Text = FH_realtime_xaxis.ToString();
                    }
                    else {; }

                    if (FH_PositionZ_cB.Checked)
                    {

                        SendControlChange(1, 30, FH_realtime_zaxis);
                        FH_MidiCC_ZPos.Text = FH_realtime_zaxis.ToString();
                    }
                    else {; }



                    if (FH_Yaw_cB.Checked)
                    {
                        SendControlChange(1, 31, _FHyaw);
                        FH_YawMidiCC.Text = _FHyaw.ToString();
                    }
                    else {; }

                    if (FH_Roll_cB.Checked)
                    {
                        SendControlChange(1, 85, _FHroll);
                        FH_RollMidiCC.Text = _FHroll.ToString();
                    }
                    else {; }

                    if (FH_Pitch_cB.Checked)
                    {
                        SendControlChange(1, 86, _FHpitch);
                        FH_PitchMidiCC.Text = _FHpitch.ToString();
                    }
                    else {; }

                    if (FH_fingerspread_cB.Checked)
                    {
                        SendControlChange(1, 87, FH_Angle_fingerspread);
                        FH_MidiCC_fingerSpread.Text = FH_Angle_fingerspread.ToString();
                    }
                    else {; }

                    if (FH_timeVisible_cB.Checked)
                    {
                        SendControlChange(1, 88, FH_timeVisible);
                        FH_MidiCC_tV.Text = FH_timeVisible.ToString();
                    }
                    else {; }


                    //********************************************user defined CC number *****************************************//
                    if (UserDefinedCC_cB.Checked)
                    {

                        if (ActivateGrab_box.Checked)
                        {
                            if (FH_grabtB.Text != "")
                            {
                                try
                                {

                                    int FHuser_grab = int.Parse(FH_grabtB.Text);

                                    if (FHuser_grab > 0 && FHuser_grab < 128)
                                    {

                                        SendControlChange(1, FHuser_grab, _grabstrengthOne);
                                    }
                                    else
                                    {
                                        FH_grabtB.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");
                                    }
                                }
                                catch (FormatException)
                                {
                                    FH_grabtB.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for First hand Grab");
                                }


                            }
                            else {; }

                        }
                        else {; }

                        if (ActivateGrab_box.Checked)
                        {
                            if (SH_grab_cB.Checked)
                            {
                                //SH_grabtB.Enabled = true;
                                if (SH_grabtB.Text != "")
                                {

                                    try
                                    {
                                        int SHuser_grab = int.Parse(SH_grabtB.Text);

                                        if (SHuser_grab > 0 && SHuser_grab < 128)
                                        {
                                            SendControlChange(1, SHuser_grab, _grabstrengthTwo);
                                        }
                                        else
                                        {
                                            SH_grabtB.Text = "";
                                            MessageBox.Show("Please Enter a numeric value between 1 and 127");

                                        }

                                    }
                                    catch (FormatException)
                                    {
                                        SH_grabtB.Text = "";
                                        MessageBox.Show("Please Enter an integer for Second hand Grab");
                                    }

                                }
                                else {; }

                            }
                            else {; }
                        }
                        else {; }



                        if (FH_speed_cB.Checked)
                        {

                            //FH_speedtB.Enabled = true;

                            if (FH_speedtB.Text != "")
                            {
                                try
                                {

                                    int FH_user_speed = int.Parse(FH_speedtB.Text);

                                    if (FH_user_speed > 0 && FH_user_speed < 128)
                                    {

                                        SendControlChange(1, FH_user_speed, FH_realtimeSpeed);

                                    }
                                    else
                                    {
                                        FH_speedtB.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");

                                    }

                                }
                                catch (FormatException)
                                {
                                    FH_speedtB.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for First hand Speed");

                                }


                            }
                            else {; }

                        }
                        else {; }

                        if (SH_speed_cB.Checked)
                        {
                            //SH_speedtB.Enabled = true;

                            if (SH_speedtB.Text != "")
                            {
                                try
                                {
                                    int SH_user_speed = int.Parse(SH_speedtB.Text);

                                    if (SH_user_speed > 0 && SH_user_speed < 128)
                                    {

                                        SendControlChange(1, SH_user_speed, SH_realtimeSpeed);
                                    }
                                    else
                                    {
                                        SH_speedtB.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");

                                    }

                                }
                                catch (FormatException)
                                {
                                    SH_speedtB.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for Second hand Speed");

                                }

                            }
                            else {; }
                        }
                        else {; }

                        if (FH_PositionY_cB.Checked)
                        {
                            // FH_posYtB.Enabled = true;
                            if (FH_posYtB.Text != "")
                            {
                                try
                                {
                                    int FH_user_PosY = int.Parse(FH_posYtB.Text);

                                    if (FH_user_PosY > 0 && FH_user_PosY < 128)
                                    {
                                        SendControlChange(1, FH_user_PosY, FH_realtime_yaxis);
                                    }
                                    else
                                    {
                                        FH_posYtB.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");
                                    }

                                }
                                catch (FormatException)
                                {
                                    FH_posYtB.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for First hand Position Y-axis");

                                }
                            }
                            else {; }
                        }
                        else {; }

                        if (SH_PositionY_cB.Checked)
                        {
                            // SH_posYtB.Enabled = true;
                            if (SH_posYtB.Text != "")
                            {
                                try
                                {
                                    int SH_user_PosY = int.Parse(SH_posYtB.Text);

                                    if (SH_user_PosY > 0 && SH_user_PosY < 128)
                                    {

                                        SendControlChange(1, SH_user_PosY, SH_realtime_yaxis);
                                    }
                                    else
                                    {
                                        SH_posYtB.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");
                                    }

                                }
                                catch (FormatException)
                                {
                                    SH_posYtB.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for Second hand Position Y-axis");

                                }
                            }
                            else {; }
                        }
                        else {; }

                        if (FH_PositionX_cB.Checked)
                        {
                            //FH_posXtB.Enabled = true;
                            if (FH_posXtB.Text != "")
                            {
                                try
                                {
                                    int FH_user_PosX = int.Parse(FH_posXtB.Text);
                                    if (FH_user_PosX > 0 && FH_user_PosX < 128)
                                    {
                                        SendControlChange(1, FH_user_PosX, FH_realtime_xaxis);
                                    }
                                    else
                                    {
                                        FH_posXtB.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");
                                    }

                                }
                                catch (FormatException)
                                {
                                    FH_posXtB.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for First hand Position X-axis");
                                }
                            }
                            else {; }
                        }
                        else {; }

                        if (SH_PositionX_cB.Checked)
                        {
                            //SH_posXtB.Enabled = true;
                            if (SH_posXtB.Text != "")
                            {
                                try
                                {
                                    int SH_user_PosX = int.Parse(SH_posXtB.Text);
                                    if (SH_user_PosX > 0 && SH_user_PosX < 128)
                                    {
                                        SendControlChange(1, SH_user_PosX, SH_realtime_xaxis);
                                    }
                                    else
                                    {
                                        SH_posXtB.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");
                                    }

                                }
                                catch (FormatException)
                                {
                                    SH_posXtB.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for Second hand Position X-axis");
                                }

                            }
                            else {; }
                        }
                        else {; }

                        if (FH_PositionZ_cB.Checked)
                        {
                            //FH_posZtB.Enabled = true;
                            if (FH_posZtB.Text != "")
                            {
                                try
                                {
                                    int FH_user_PosZ = int.Parse(FH_posZtB.Text);
                                    if (FH_user_PosZ > 0 && FH_user_PosZ < 128)
                                    {

                                        SendControlChange(1, FH_user_PosZ, FH_realtime_zaxis);
                                    }
                                    else
                                    {
                                        FH_posZtB.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");
                                    }

                                }
                                catch (FormatException)
                                {
                                    FH_posZtB.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for First hand Position Z-axis");
                                }

                            }
                            else {; }
                        }
                        else {; }

                        if (SH_PositionZ_cB.Checked)
                        {
                            // SH_posZtB.Enabled = true;
                            if (SH_posZtB.Text != "")
                            {
                                try
                                {
                                    int SH_user_PosZ = int.Parse(SH_posZtB.Text);
                                    if (SH_user_PosZ > 0 && SH_user_PosZ < 128)
                                    {

                                        SendControlChange(1, SH_user_PosZ, SH_realtime_zaxis);
                                    }
                                    else
                                    {
                                        SH_posZtB.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");
                                    }

                                }
                                catch (FormatException)
                                {
                                    SH_posZtB.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for Second hand Position Z-axis");
                                }

                            }
                            else {; }
                        }
                        else {; }

                        if (FH_Yaw_cB.Checked)
                        {
                            //  FH_yawtB.Enabled = true;
                            if (FH_yawtB.Text != "")
                            {
                                try
                                {
                                    int FH_user_yaw = int.Parse(FH_yawtB.Text);
                                    if (FH_user_yaw > 0 && FH_user_yaw < 128)
                                    {
                                        SendControlChange(1, FH_user_yaw, _FHyaw);
                                    }
                                    else
                                    {
                                        FH_yawtB.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");
                                    }

                                }
                                catch (FormatException)
                                {
                                    FH_yawtB.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for First hand Yaw");
                                }
                            }
                            else {; }
                        }
                        else {; }

                        if (SH_Yaw_cB.Checked)
                        {
                            // SH_yawtB.Enabled = true;
                            if (SH_yawtB.Text != "")
                            {
                                try
                                {
                                    int SH_user_yaw = int.Parse(SH_yawtB.Text);
                                    if (SH_user_yaw > 0 && SH_user_yaw < 128)
                                    {

                                        SendControlChange(1, SH_user_yaw, _SHyaw);
                                    }
                                    else
                                    {
                                        SH_yawtB.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");
                                    }

                                }
                                catch (FormatException)
                                {
                                    SH_yawtB.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for Second hand Yaw");
                                }
                            }
                            else {; }
                        }
                        else {; }

                        if (FH_Roll_cB.Checked)
                        {
                            // FH_rolltB.Enabled = true;
                            if (FH_rolltB.Text != "")
                            {
                                try
                                {
                                    int FH_user_roll = int.Parse(FH_rolltB.Text);
                                    if (FH_user_roll > 0 && FH_user_roll < 128)
                                    {
                                        SendControlChange(1, FH_user_roll, _FHroll);
                                    }
                                    else
                                    {
                                        FH_rolltB.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");
                                    }

                                }
                                catch (FormatException)
                                {
                                    FH_rolltB.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for First hand Roll");
                                }
                            }
                            else {; }
                        }
                        else {; }

                        if (SH_Roll_cB.Checked)
                        {
                            // SH_rolltB.Enabled = true;
                            if (SH_rolltB.Text != "")
                            {
                                try
                                {
                                    int SH_user_roll = int.Parse(SH_rolltB.Text);
                                    if (SH_user_roll > 0 && SH_user_roll < 128)
                                    {
                                        SendControlChange(1, SH_user_roll, _SHroll);
                                    }
                                    else
                                    {
                                        SH_rolltB.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");
                                    }

                                }
                                catch (FormatException)
                                {
                                    SH_rolltB.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for Second hand Roll");
                                }
                            }
                            else {; }
                        }
                        else {; }

                        if (FH_Pitch_cB.Checked)
                        {
                            // FH_pitchtB.Enabled = true;
                            if (FH_pitchtB.Text != "")
                            {
                                try
                                {
                                    int FH_user_pitch = int.Parse(FH_pitchtB.Text);
                                    if (FH_user_pitch > 0 && FH_user_pitch < 128)
                                    {
                                        SendControlChange(1, FH_user_pitch, _FHpitch);
                                    }
                                    else
                                    {
                                        FH_pitchtB.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");
                                    }

                                }
                                catch (FormatException)
                                {
                                    FH_pitchtB.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for First hand Pitch");
                                }
                            }
                            else {; }
                        }
                        else {; }

                        if (SH_Pitch_cB.Checked)
                        {
                            /// SH_pitchtB.Enabled = true;
                            if (SH_pitchtB.Text != "")
                            {
                                try
                                {
                                    int SH_user_pitch = int.Parse(SH_pitchtB.Text);
                                    if (SH_user_pitch > 0 && SH_user_pitch < 128)
                                    {
                                        SendControlChange(1, SH_user_pitch, _SHpitch);
                                    }
                                    else
                                    {
                                        SH_pitchtB.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");
                                    }

                                }
                                catch (FormatException)
                                {
                                    SH_pitchtB.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for Second hand Pitch");
                                }
                            }
                            else {; }
                        }
                        else {; }

                        if (FH_fingerspread_cB.Checked)
                        {
                            // FH_FinSptB.Enabled = true;
                            if (FH_FinSptB.Text != "")
                            {
                                try
                                {
                                    int FH_userFS = int.Parse(FH_FinSptB.Text);
                                    if (FH_userFS > 0 && FH_userFS < 128)
                                    {
                                        SendControlChange(1, FH_userFS, FH_Angle_fingerspread);
                                    }
                                    else
                                    {
                                        FH_FinSptB.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");
                                    }

                                }
                                catch (FormatException)
                                {
                                    FH_FinSptB.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for First hand Finger Spread");
                                }
                            }
                            else {; }
                        }
                        else {; }

                        if (SH_fingerspread_cB.Checked)
                        {
                            //SH_FinSptB.Enabled = true;
                            if (SH_FinSptB.Text != "")
                            {
                                try
                                {
                                    int SH_userFS = int.Parse(SH_FinSptB.Text);
                                    if (SH_userFS > 0 && SH_userFS < 128)
                                    {
                                        SendControlChange(1, SH_userFS, SH_Angle_fingerspread);
                                    }
                                    else
                                    {
                                        SH_FinSptB.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");
                                    }

                                }
                                catch (FormatException)
                                {
                                    SH_FinSptB.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for Second hand Finger Spread");
                                }
                            }
                            else {; }
                        }
                        else {; }

                        if (FH_timeVisible_cB.Checked)
                        {
                            // FH_timeVstb.Enabled = true;
                            if (FH_timeVstb.Text != "")
                            {
                                try
                                {
                                    int FH_userTV = int.Parse(FH_timeVstb.Text);
                                    if (FH_userTV > 0 && FH_userTV < 128)
                                    {
                                        SendControlChange(1, FH_userTV, FH_timeVisible);
                                    }
                                    else
                                    {
                                        FH_timeVstb.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");
                                    }

                                }
                                catch (FormatException)
                                {
                                    FH_timeVstb.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for First hand Time-Visible");
                                }
                            }
                            else {; }
                        }
                        else {; }

                        if (SH_timeVisible_cB.Checked)
                        {
                            //SH_timeVstb.Enabled = true;
                            if (SH_timeVstb.Text != "")
                            {
                                try
                                {
                                    int SH_userTV = int.Parse(SH_timeVstb.Text);
                                    if (SH_userTV > 0 && SH_userTV < 128)
                                    {
                                        SendControlChange(1, SH_userTV, SH_timeVisible);
                                    }
                                    else
                                    {
                                        SH_timeVstb.Text = "";
                                        MessageBox.Show("Please Enter a numeric value between 1 and 127");
                                    }

                                }
                                catch (FormatException)
                                {
                                    SH_timeVstb.Text = "";
                                    MessageBox.Show("Please Enter a numeric value for First hand Time-Visible");
                                }
                            }
                            else {; }
                        }
                        else {; }

                    }
                }

            }
            else
            {
                SecondHand_MiDiCC_groupBox.Enabled = false;
                Firsthand_MiDiCC_groupBox.Enabled = false;
                UserDefinedCC_groupBox.Enabled = false;
            }


            if (ActivateGrab_box.Checked)
            {
                grab_gBox.Enabled = true;

                SendControlChange(1, 89, _grabstrengthOne);
                MidiCC_grab1lb.Text = _grabstrengthOne.ToString();
                if (SH_grab_cB.Checked)
                {
                    SendControlChange(1, 90, _grabstrengthTwo);
                    MidiCC_grab2lb.Text = _grabstrengthTwo.ToString();
                }
            }
            else
            {
                grab_gBox.Enabled = false;

                MidiCC_grab1lb.Text = "";
                MidiCC_grab2lb.Text = "";

            }
        }



        public void SendControlChange(int MidiChannel, int controlnumber, int value)
        {
            if (outDevice != null)
            {
                //if() i want to select what to send either a note on or controller 
                ChannelMessage msg = new ChannelMessage(ChannelCommand.Controller, MidiChannel, controlnumber, value);
                outDevice.Send(msg);
            }

        }

        private void stop_button_Click(object sender, EventArgs e)
        {
            DialogResult sluiten = MessageBox.Show("Do you really want to quit?", "Quit", MessageBoxButtons.YesNo);
            if (sluiten == DialogResult.Yes)
            {
                controller.RemoveListener(listener);
               // controller.Dispose();


                Application.Exit();
            }
            else if (sluiten == DialogResult.No)
            {
                sluiten = DialogResult.Cancel;
                //e.Cancel = true;
            }
        }

        private void GUILeapDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult form_sluiten = MessageBox.Show("Do you really want to quit?", "Quit", MessageBoxButtons.YesNo);
            if (form_sluiten == DialogResult.Yes)
            {
                controller.RemoveListener(listener);
              //controller.Dispose();

                Application.Exit();

            }
            else if (form_sluiten == DialogResult.No)
            {
                form_sluiten = DialogResult.Cancel;
                //e.Cancel = true;
            }

        }
    }

}


