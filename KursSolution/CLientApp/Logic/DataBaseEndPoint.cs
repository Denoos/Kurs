using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using CLientApp.Model;
using CLientApp.Models;

namespace CLientApp.Logic
{
    public class DataBaseEndPoint
    {
        private static DataBaseEndPoint instance;
        public static DataBaseEndPoint Instance { get => instance ??= new(); }
        private readonly HttpClient _client;

        private User CurrentAccount;

        public DataBaseEndPoint()
            => _client = new() { BaseAddress = new Uri("https://localhost:7230/api/") };

        public async Task<bool> Login(User user)
        {
            try
            {
                var result = false;
                CurrentAccount ??= user;

                if (user is null ||
                    string.IsNullOrEmpty(user.Login) ||
                    string.IsNullOrEmpty(user.Password) ||
                    string.IsNullOrWhiteSpace(user.Login) ||
                    string.IsNullOrWhiteSpace(user.Password))
                    return result;

                var responce = await _client.GetFromJsonAsync<TokEnRole>($"Auth/Authorise?login={user.Login}&password={user.Password}");

                if (responce is null)
                    return false;

                if (responce.Title is not null && !string.IsNullOrWhiteSpace(responce.Token))
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responce.Token);
                    result = true;
                }


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

                user.IdRoleNavigation = new() { Ttle = "0" };

                var responceCode = await _client.PostAsJsonAsync($"Auth/Register", user);

                var responce = await responceCode.Content.ReadFromJsonAsync<TokEnRole>();

                if (responce is null)
                    return false;

                if (await Login(user))
                {
                    MessageBox.Show("Вы успешно зарегистрировались!", "Успех!");
                    result = true;
                }

                return result;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка добавления!", "Ошибка!");
                return false;
            }
        }

        public List<Ppe> GetAllPpes()
        {
            try
            {
                var responce = _client.GetFromJsonAsync<IEnumerable<Ppe>>($"Ppes/GetPpes");
                return responce.Result.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return [];
            }
        }

        public List<Model.Condition> GetAllConditions()
        {
            try
            {
                var responce = _client.GetFromJsonAsync<IEnumerable<Model.Condition>>($"Conditions/GetConditions");
                return responce.Result.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return [];
            }
        }

        public List<Post> GetAllPosts()
        {
            try
            {
                var responce = _client.GetFromJsonAsync<IEnumerable<Post>>($"Posts/GetPosts");
                return responce.Result.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return [];
            }
        }


        public List<PpeType> GetAllPpeTypes()
        {
            try
            {
                var responce = _client.GetFromJsonAsync<IEnumerable<PpeType>>($"PpeTypes/GetPpeTypes");
                return responce.Result.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return [];
            }
        }

        public List<Person> GetAllPersons()
        {
            try
            {
                var responce = _client.GetFromJsonAsync<IEnumerable<Person>>($"People/GetPersons");
                return responce.Result.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return [];
            }
        }

        public List<Status> GetAllStatuses()
        {
            try
            {
                var responce = _client.GetFromJsonAsync<IEnumerable<Status>>($"Status/GetStatuses");
                return responce.Result.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return [];
            }
        }

        public List<User> GetAllUsers()
        {
            try
            {
                var responce = _client.GetFromJsonAsync<IEnumerable<User>>($"User/GetUsers");
                return responce.Result.ToList();
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    case "One or more errors occurred. (Response status code does not indicate success: 403 (Forbidden).)":
                        MessageBox.Show("Ошибка доступа, Вы не можете получить доступ к этой части приложения!", "Ошибка!");
                        break;

                    default:
                        MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                        break;
                }
                return [];
            }
        }

        public List<Role> GetAllRoles()
        {
            try
            {
                var responce = _client.GetFromJsonAsync<IEnumerable<Role>>($"User/GetPosts");
                return responce.Result.ToList();
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    case "One or more errors occurred. (Response status code does not indicate success: 403 (Forbidden).)":
                        MessageBox.Show("Ошибка доступа, Вы не можете получить доступ к этой части приложения!", "Ошибка!");
                        break;

                    default:
                        MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                        break;
                }
                return [];
            }
        }

        public async Task DeletePpe(Ppe selectedItem)
        {
            if (selectedItem is null)
                return;

            try
            {
                var responce = await _client.DeleteAsync($"Ppes/DeletePpe?id={selectedItem.Id}");

                if (responce != null && responce.StatusCode == System.Net.HttpStatusCode.OK)
                    MessageBox.Show("Объект удален!", "Успех!");
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return;
            }
        }

        public async Task DeleteCondition(Model.Condition selectedItem)
        {
            if (selectedItem is null)
                return;

            try
            {
                var responce = await _client.DeleteAsync($"Conditions/DeleteCondition?id={selectedItem.Id}");

                if (responce != null && responce.StatusCode == System.Net.HttpStatusCode.OK)
                    MessageBox.Show("Объект удален!", "Успех!");
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return;
            }
        }

        public async Task DeletePost(Post selectedItem)
        {
            if (selectedItem is null)
                return;

            try
            {
                var responce = await _client.DeleteAsync($"Posts/DeletePost?id={selectedItem.Id}");

                if (responce != null && responce.StatusCode == System.Net.HttpStatusCode.OK)
                    MessageBox.Show("Объект удален!", "Успех!");
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return;
            }
        }

        public async Task DeletePpeType(PpeType selectedItem)
        {
            if (selectedItem is null)
                return;

            try
            {
                var responce = await _client.DeleteAsync($"PpeTypes/DeletePpeType?id={selectedItem.Id}");

                if (responce != null && responce.StatusCode == System.Net.HttpStatusCode.OK)
                    MessageBox.Show("Объект удален!", "Успех!");
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return;
            }
        }

        public async Task DeleteStatus(Status selectedItem)
        {
            if (selectedItem is null)
                return;

            try
            {
                var responce = await _client.DeleteAsync($"Status/DeleteStatus?id={selectedItem.Id}");

                if (responce != null && responce.StatusCode == System.Net.HttpStatusCode.OK)
                    MessageBox.Show("Объект удален!", "Успех!");
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return;
            }
        }

        public async Task DeletePerson(Person selectedItem)
        {
            if (selectedItem is null)
                return;

            try
            {
                var responce = await _client.DeleteAsync($"People/DeletePerson?id={selectedItem.Id}");

                if (responce != null && responce.StatusCode == System.Net.HttpStatusCode.OK)
                    MessageBox.Show("Объект удален!", "Успех!");
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return;
            }
        }

        public async Task DeleteUser(User selectedItem)
        {
            if (selectedItem is null)
                return;

            try
            {
                var responce = await _client.DeleteAsync($"User/DeleteUser?id={selectedItem.Id}");

                if (responce != null && responce.StatusCode == System.Net.HttpStatusCode.OK)
                    MessageBox.Show("Объект удален!", "Успех!");
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    case "One or more errors occurred. (Response status code does not indicate success: 403 (Forbidden).)":
                        MessageBox.Show("Ошибка доступа, Вы не можете получить доступ к этой части приложения!", "Ошибка!");
                        break;

                    default:
                        MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                        break;
                }
                return;
            }
        }

        public async Task<bool> AddCondition(Model.Condition item)
        {
            var result = false;

            if (item is null)
                return result;

            try
            {
                var responce = await _client.PostAsJsonAsync<Model.Condition>($"Conditions/PostCondition", item);
                var list = GetAllConditions();

                if (responce is null)
                    return result;

                if(list.Contains(item))
                    result = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
            }

            return result;
        }

        public async Task<bool> EditCondition(Model.Condition item)
        {
            var result = false;

            if (item is null)
                return result;

            try
            {
                var responce = await _client.PutAsJsonAsync($"Conditions/PutCondition", item);
                var list = GetAllConditions();

                if (responce is null)
                    return result;

                if (list.Contains(item))
                    result = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
            }

            return result;
        }

        public async Task<bool> AddPost(Post item)
        {
            var result = false;

            if (item is null)
                return result;

            try
            {
                var responce = await _client.PostAsJsonAsync<Post>($"Posts/PostPost", item);
                var list = GetAllPosts();

                if (responce is null)
                    return result;

                if (list.Contains(item))
                    result = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
            }

            return result;
        }

        public async Task<bool> EditPost(Post item)
        {
            var result = false;

            if (item is null)
                return result;

            try
            {
                var responce = await _client.PutAsJsonAsync<Post>($"Posts/PutPost", item);
                var list = GetAllPosts();

                if (responce is null)
                    return result;

                if (list.Contains(item))
                    result = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
            }

            return result;
        }

        public async Task<bool> AddPpeType(PpeType item)
        {
            var result = false;

            if (item is null)
                return result;

            try
            {
                var responce = await _client.PostAsJsonAsync<PpeType>($"PpeTypes/PostPpeType", item);
                var list = GetAllPpeTypes();

                if (responce is null)
                    return result;

                if (list.Contains(item))
                    result = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
            }

            return result;
        }

        public async Task<bool> EditPpeType(PpeType item)
        {
            var result = false;

            if (item is null)
                return result;

            try
            {
                var responce = await _client.PutAsJsonAsync<PpeType>($"PpeTypes/PutPpeType", item);
                var list = GetAllPpeTypes();

                if (responce is null)
                    return result;

                if (list.Contains(item))
                    result = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
            }

            return result;
        }

        public async Task<bool> AddStatus(Status item)
        {
            var result = false;

            if (item is null)
                return result;

            try
            {
                var responce = await _client.PostAsJsonAsync<Status>($"Status/PostStatus", item);
                var list = GetAllStatuses();

                if (responce is null)
                    return result;

                if (list.Contains(item))
                    result = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
            }

            return result;
        }

        public async Task<bool> EditStatus(Status item)
        {
            var result = false;

            if (item is null)
                return result;

            try
            {
                var responce = await _client.PutAsJsonAsync<Status>($"Status/PutStatus", item);
                var list = GetAllStatuses();

                if (responce is null)
                    return result;

                if (list.Contains(item))
                    result = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
            }

            return result;
        }

        public async Task<bool> AddUser(User item)
        {
            var result = false;

            if (item is null)
                return result;

            try
            {
                var responce = await _client.PostAsJsonAsync<User>($"User/PostUser", item);
                var list = GetAllUsers();

                if (responce is null)
                    return result;

                if (list.Contains(item))
                    result = true;
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    case "One or more errors occurred. (Response status code does not indicate success: 403 (Forbidden).)":
                        MessageBox.Show("Ошибка доступа, Вы не можете получить доступ к этой части приложения!", "Ошибка!");
                        break;

                    default:
                        MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                        break;
                }
            }

            return result;
        }

        public async Task<bool> EditUser(User item)
        {
            var result = false;

            if (item is null)
                return result;

            try
            {
                var responce = await _client.PutAsJsonAsync<User>($"User/PutUser", item);
                var list = GetAllUsers();

                if (responce is null)
                    return result;

                if (list.Contains(item))
                    result = true;
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    case "One or more errors occurred. (Response status code does not indicate success: 403 (Forbidden).)":
                        MessageBox.Show("Ошибка доступа, Вы не можете получить доступ к этой части приложения!", "Ошибка!");
                        break;

                    default:
                        MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                        break;
                }
            }

            return result;
        }

        public async Task<bool> EditPpe(Ppe item)
        {
            var result = false;

            if (item is null)
                return result;

            try
            {
                var responce = await _client.PutAsJsonAsync<Ppe>($"Ppes/PutPpe", item);
                var list = GetAllPpes();

                if (responce is null)
                    return result;

                if (list.Contains(item))
                    result = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
            }

            return result;
        }

        public async Task<bool> AddPpe(Ppe item)
        {
            var result = false;

            if (item is null)
                return result;

            try
            {
                var responce = await _client.PostAsJsonAsync<Ppe>($"Ppes/PostPpe", item);
                var list = GetAllPpes();

                if (responce is null)
                    return result;

                if (list.Contains(item))
                    result = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
            }

            return result;
        }

        public async Task<bool> AddPerson(Person item)
        {
            var result = false;

            if (item is null)
                return result;

            try
            {
                var responce = await _client.PostAsJsonAsync<Person>($"People/PostPerson", item);
                var list = GetAllPersons();

                if (responce is null)
                    return result;

                if (list.Contains(item))
                    result = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
            }

            return result;
        }

        public async Task<bool> EditPerson(Person item)
        {
            var result = false;

            if (item is null)
                return result;

            try
            {
                var responce = await _client.PutAsJsonAsync<Person>($"People/PutPerson", item);
                var list = GetAllPersons();

                if (responce is null)
                    return result;

                if (list.Contains(item))
                    result = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
            }

            return result;
        }
    }
}