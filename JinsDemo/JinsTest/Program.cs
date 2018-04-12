using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace JinsTest
{
    public class Program
    {
        private static System.Timers.Timer timer;
        static string dongleConnectedPort;
        static bool measureDataThreadLoop = true;
        static Mutex graphMutex = new Mutex();

        //Data management class
        static DataManager dataManager = new DataManager();

        static void Main(string[] args)
        {
            detect();
            Console.WriteLine("Press a key");
            Console.ReadKey();
            connect();
        }

        public static void detect()
        {
            //Scan the port
            Console.WriteLine("Try to find on which port is connectd the dongle...");
            dataManager.ScanComPort();
            System.Threading.Thread.Sleep(15000);

            //On which port is connected the dongle
            if (dataManager.DonglePortList.Count == 1)
            {
                dongleConnectedPort = dataManager.DonglePortList[0];
                Console.WriteLine("The dongle is connected on port\t" + dongleConnectedPort + "\n");
            }

            else if (dataManager.DonglePortList.Count > 1)
            {
                Console.WriteLine("More than one dongle connected\n");
                for (int i = 0; i < dataManager.DonglePortList.Count; i++)
                {
                    Console.WriteLine("There is one dongle on port " + dataManager.DonglePortList[i]);
                }

                Console.WriteLine("We will connect to the dongle on port" + dataManager.DonglePortList[0] + "\n");
                Console.WriteLine("OK?\n");

                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Is the dongle connected?\n");
                return;
            }


            Console.WriteLine("Turn on the device\n");
        }


        public static void connect()
        {

            //If the dongle is connected we can try to open the corresponding port 
            Console.WriteLine("Try to open the port...");
            if (dataManager.ConnectComPort(dataManager.DonglePortList[0]))
            {
                Console.WriteLine("Success to open the port\n");
            }
            else
            {
                Console.WriteLine("Fail to open the port\n");
                Console.WriteLine("Press any key to quit");
                Console.ReadKey();
                return;
            }


            //We scan which bluetooth device is available
            Console.WriteLine("Try to scan bluetooth device...");
            if (dataManager.ScanBluetoothDevice() == false)
            {
                Console.WriteLine("Fail to scan bluetooth device\n");
                Console.WriteLine("Press any key to quit");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine("Success to scan bluetooth device\n");

            }

            //We wait until we get the bluetooth MAC address of the device
            Console.WriteLine("Try to connect to the device...");
            bool timerSet = false;
            while (true)
            {

                //We set the timer just once
                if (timerSet == false)
                {
                    timer = new System.Timers.Timer(8000);
                    timer.Elapsed += timeOutEvent;
                    timer.AutoReset = true;
                    timer.Enabled = true;
                    timerSet = true; ;
                }
                if (dataManager.macAdress != "") { break; }       //We try to get the MAC device's MAC address during 10 seconds, after 10 seconds of failure we stop the application
            }
            //If we successfully get the MAC address we stop the timer
            timer.Stop();
            timer.Dispose();


            //To connect to the device
            if (dataManager.ConnectBluetooth(dataManager.macAdress) == false)
            {

                Console.WriteLine("Fail To Connect Bluetooth\n");
                Console.WriteLine("Press any key to quit");
                Console.ReadKey();
                return;
            }
            else { Console.WriteLine("Device connected successfully\n"); }


            //To print the data
            Console.WriteLine("Try to start measurment...");
            if (dataManager.MeasureStart((byte)(2), (byte)(1), (byte)0, (byte)0) == false)
            {
                Console.WriteLine("Fail to start measurment\n");
                Console.WriteLine("Press any key to quit");
                Console.ReadKey();
                return;
            }
            else { Console.WriteLine("Start measurment success\n"); }

            Thread measureDataThread = new Thread(() => recvMeasureData(measureDataThreadLoop, dataManager, graphMutex));  //To pass arguments to the thread related method
            measureDataThreadLoop = true;
            measureDataThread.Name = "Measure data thread (Sensor)";
            measureDataThread.Start();

            Console.WriteLine("Please wait until measurment start");

            System.Threading.Thread.Sleep(8000);
        }

        public static void recvMeasureData(bool measureDataThreadLoop, DataManager dataManager, Mutex graphMutex)
        {
            int receiveInterval = Constants.RECEIVE_PROC_INTERVAL;
            while (measureDataThreadLoop)
            {
                List<MeasureBean> sensorDataList = dataManager.GetSensorData();

                if (sensorDataList != null)
                {
                    try
                    {
                        graphMutex.WaitOne();

                        foreach (MeasureBean bean in sensorDataList)
                        {
                            Console.WriteLine(bean.Y[6] + "\t" + bean.Y[7] + "\t" + bean.Y[8] + "\t" + bean.Y[9]);
                        }

                    }

                    finally
                    {
                        // Mutexロック解放
                        graphMutex.ReleaseMutex();
                    }
                }
                Thread.Sleep(receiveInterval);

            }
        }

        static public void bluetoothMethod(string macAddress)
        {
            Console.WriteLine("BLUETOOTH ADDRESS IS " + macAddress);
            return;
        }

        public static void timeOutEvent(Object source, ElapsedEventArgs e)
        {


            timer.Stop();
            timer.Dispose();
            Console.WriteLine("Fail to connect to the device");
            Console.WriteLine("\nIs the device in bluetooth detection mode? (bliking blue led)\n");
            Console.Write("Press any key to continue\n");
            Console.ReadKey();
            Environment.Exit(0);

        }

    }
}
