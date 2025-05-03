using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CLientApp.Models;

namespace CLientApp.Logic
{
    public class DataBaseEndPoint
    {
        private static DataBaseEndPoint instance;
        public static DataBaseEndPoint Instance { get => instance ??= new(); }
        HttpClient _client;
        //=> _client = nexw() { BaseAddress = new Uri("") };

        public DataBaseEndPoint()
        { }

        public bool Login(User user)
        {
            return true;
        }

        public bool Register(User user)
        {
            return true;
        }

        public ObservableCollection<Ppe>  GetAllPpes()
        {
            return [];
        }
    }
}
