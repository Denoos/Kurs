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

        public ObservableCollection<Status> GetAllStatuses()
        {
            return [];
        }

        internal ObservableCollection<User> GetAllUsers()
        {
            throw new NotImplementedException();
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
        
        public void DeleteStatus(Status selectedItem)
        {
            throw new NotImplementedException();
        }

        public void DeletePerson(Person selectedItem)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(User selectedItem)
        {
            throw new NotImplementedException();
        }

        public bool AddCondition(Condition item)
        {
            return item.Title == "nigga";
        }

        public bool EditCondition(Condition item)
        {
            return item.Title == "Denoos";
        }

        internal bool AddPost(Post item)
        {
            throw new NotImplementedException();
        }

        internal bool EditPost(Post item)
        {
            throw new NotImplementedException();
        }

        internal bool AddPpeType(PpeType item)
        {
            throw new NotImplementedException();
        }

        internal bool EditPpeType(PpeType item)
        {
            throw new NotImplementedException();
        }

        internal bool AddStatus(Status item)
        {
            throw new NotImplementedException();
        }

        internal bool EditStatus(Status item)
        {
            throw new NotImplementedException();
        }

        internal bool AddUser(User item)
        {
            throw new NotImplementedException();
        }

        internal bool EditUser(User item)
        {
            throw new NotImplementedException();
        }

        internal List<Role> GetAllRoles()
        {
            throw new NotImplementedException();
        }
    }
}