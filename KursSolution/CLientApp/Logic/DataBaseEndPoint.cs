using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using CLientApp.Models;

namespace CLientApp.Logic
{
    public class DataBaseEndPoint
    {
        private static DataBaseEndPoint instance;
        public static DataBaseEndPoint Instance { get => instance ??= new(); }
        private readonly HttpClient _client;

        public DataBaseEndPoint()
            => _client = new() { BaseAddress = new Uri("https://localhost:7230/api/") };

        public async Task<bool> Login(User user)
        {
            try
            {
                var result = false;
                if (user is null ||
                    string.IsNullOrEmpty(user.Login) ||
                    string.IsNullOrEmpty(user.Password) ||
                    string.IsNullOrWhiteSpace(user.Login) ||
                    string.IsNullOrWhiteSpace(user.Password))
                    return result;


                var responce = await _client.GetFromJsonAsync<TokEnRole>($"Auth/Authorise?login={user.Login}&password={user.Password}");

                if (responce is null)
                    result = false;

                return result;
            }
            catch (Exception)
            {
                MessageBox.Show("Введены неверные данные!", "Ошибка!");
                return false;
            }
        }

        public async Task<bool> Register(User user)
        {
            try
            {
                var result = false;
                if (user is null ||
                    string.IsNullOrEmpty(user.Login) ||
                    string.IsNullOrEmpty(user.Password) ||
                    string.IsNullOrWhiteSpace(user.Login) ||
                    string.IsNullOrWhiteSpace(user.Password))
                    return result;

                user.IdRole = 0;
                user.Token = "";
                user.IdRoleNavigation = new Role() { Id = 0, Ttle = "0" };

                result = true;

                var responce = await _client.PostAsJsonAsync($"Auth/Register", user);
                result = await Login(user);

                return result;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка добавления!", "Ошибка!");
                return false;
            }
        }

        public ObservableCollection<Ppe> GetAllPpes()
        {
            return [];
        }

        public ObservableCollection<Models.Condition> GetAllConditions()
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

        public void DeleteCondition(Models.Condition selectedItem)
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

        public bool AddCondition(Models.Condition item)
        {
            return item.Title == "nigga";
        }

        public bool EditCondition(Models.Condition item)
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

        internal bool EditPpe(Ppe item)
        {
            throw new NotImplementedException();
        }

        internal bool AddPpe(Ppe item)
        {
            throw new NotImplementedException();
        }

        internal bool AddPerson(Person item)
        {
            throw new NotImplementedException();
        }

        internal bool EditPerson(Person item)
        {
            throw new NotImplementedException();
        }
    }
}