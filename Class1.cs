using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;
using RestSharp;
using Newtonsoft.Json;

namespace dll1
{
    public class Class1
    {

        static public double[] H1;
        static public double Bid;
        static public string Symbol;
        static public string Holder;
        static public int accountNumber;

        [DllExport("UpdateMarket", CallingConvention = CallingConvention.StdCall)]
        public static void UpdateMarket(
                [MarshalAs(UnmanagedType.LPWStr)]String symbol,
                [MarshalAs(UnmanagedType.LPWStr)]String holder,
                int account,
                float bid)
        {
            Bid = bid;
            Symbol = symbol;
            Holder = holder;
            accountNumber = account;
        }

        
        [DllExport("Command", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static String Command()
        {
            return " ";
        }



        [DllExport("BarsPrice", CallingConvention = CallingConvention.StdCall)]
        public static void BarsPrice(double h1, int size, int index)
        {
            Array.Resize(ref H1, size);
            H1[index] = h1;
        }





        [DllExport("Predict", CallingConvention = CallingConvention.StdCall)]
        public static double Predict()
        {
            var client = new RestClient("http://localhost:5000");
            var request = new RestRequest("post");
            request.AddJsonBody(H1);
            var response = client.Post(request);
            var content = response.Content;
            dynamic jsonResponse = JsonConvert.DeserializeObject(content);
            double predict = jsonResponse.value;
            return predict;
        }
    }


    class Order
    {
        public static List<Order> orderList = new List<Order> {};
        public int ticket;
        public double openPrice = 0;
        public double targetPrice = 0;
        public double stopPrice = 0;


        public Order(int ticket, double open) { 

        }
    }
}
