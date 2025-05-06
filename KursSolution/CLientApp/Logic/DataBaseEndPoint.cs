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

        public ObservableCollection<Ppe> GetAllPpes()
        {
            return [];
        }

        public ObservableCollection<Condition> GetAllConditions()
        {
            return [];
        }

        public ObservableCollection<PpeType> GetAllTypes()
        {
            return [];
        }

        public ObservableCollection<Post> GetAllPosts()
        {
            return [];
        }

        public ObservableCollection<PpeType> GetAllPpeTypes()
        {
            return [];
        }

        public ObservableCollection<Person> GetAllPersons()
        {
            return [];
        }

        public void DeletePpe(Ppe selectedPpe)
        {
            throw new NotImplementedException();
        }

        public void DeleteCondition(Condition selectedItem)
        {
            throw new NotImplementedException();
        }

        public void DeletePost(Post selectedItem)
        {
            throw new NotImplementedException();
        }

        public void DeletePpeType(PpeType selectedItem)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Status> GetAllStatuses()
        {
            throw new NotImplementedException();
        }

        public void DeleteStatus(Status selectedItem)
        {
            throw new NotImplementedException();
        }
    }
}