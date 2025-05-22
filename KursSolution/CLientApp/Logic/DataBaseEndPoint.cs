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
        public User CurrentAccount;
        private JsonSerializerOptions _options;

        public DataBaseEndPoint()
            => _client = new() { BaseAddress = new Uri("https://localhost:7230/api/") };

        public void SetOptions(JsonSerializerOptions options)
            => this._options = options;

        public async Task<bool> CheckAdmin()
        {
            try
            {
                var result = false;

                var resp = await _client.GetAsync($"User/GetUsers");

                Task.WaitAll();

                if (resp != null && resp.StatusCode == System.Net.HttpStatusCode.OK)
                    result = true;

                return !result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return false;
            }
        }

        public async Task<bool> CheckAdminTeammate()
        {
            try
            {
                var result = false;

                var list = await GetAllConditions();
                var resp = await _client.PutAsJsonAsync($"Conditions/PutCondition", list[0], _options);

                Task.WaitAll();

                if (resp != null && resp.StatusCode == System.Net.HttpStatusCode.OK)
                    result = true;

                return !result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return false;
            }
        }


        public async Task<bool> PostDefaultPassword(string item)
        {
            var result = false;

            if (item is null)
                return result;

            try
            {
                var responce = await _client.GetAsync($"User/PostDefaultPassword?someStrongString={item}");

                if (responce is null)
                    return result;

                if (responce.StatusCode == System.Net.HttpStatusCode.OK)
                    result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Код был введен неверно! Сис. админу было отправлено уведомление о попытке изменить данные!", "Ошибка!");
                MessageBox.Show(ex.Message);
            }

            return result;
        }

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

                var resp = await _client.GetAsync($"Auth/Authorise?login={user.Login}&password={user.Password}");
                var responce = await resp.Content.ReadFromJsonAsync<TokEnRole>(_options);

                if (responce is null)
                    return false;
                else
                {
                    CurrentAccount.IdRoleNavigation = responce.Title;
                    CurrentAccount.IdRole = responce.Title.Id;
                }

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

                var resp = await _client.PostAsJsonAsync($"Auth/Register", user);
                var responce = await resp.Content.ReadFromJsonAsync<TokEnRole>(_options);

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

        public async Task<List<Ppe>> GetAllPpes()
        {
            try
            {
                var resp = _client.GetAsync($"Ppes/GetPpes").Result;
                var responce = resp.Content.ReadFromJsonAsync<IEnumerable<Ppe>>(_options);

                return responce.Result.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return [];
            }
        }

        public async Task<List<Model.Condition>> GetAllConditions()
        {
            try
            {
                var resp = await _client.GetAsync($"Conditions/GetConditions");
                var responce = resp.Content.ReadFromJsonAsync<IEnumerable<Model.Condition>>(_options);

                return responce.Result.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return [];
            }
        }

        public async Task<List<Post>> GetAllPosts()
        {
            try
            {
                var resp = await _client.GetAsync($"Posts/GetPosts");
                var responce = resp.Content.ReadFromJsonAsync<IEnumerable<Post>>(_options);

                var result = responce.Result.ToList();

                return result;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return [];
            }
        }

        public async Task<List<PpeType>> GetAllPpeTypes()
        {
            try
            {
                var resp = await _client.GetAsync($"PpeTypes/GetPpeTypes");
                var responce = resp.Content.ReadFromJsonAsync<IEnumerable<PpeType>>(_options);

                return responce.Result.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return [];
            }
        }

        public async Task<List<Person>> GetAllPersons()
        {
            try
            {
                var resp = await _client.GetAsync($"People/GetPersons");
                var responce = resp.Content.ReadFromJsonAsync<IEnumerable<Person>>(_options);

                return responce.Result.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return [];
            }
        }

        public async Task<Person> GetPerson(int? id)
        {
            var a = await GetAllPersons();
            if (id == null)
                return new Person();
            return a.FirstOrDefault(s => s.Id == id);
        }

        public async Task<List<Status>> GetAllStatuses()
        {
            try
            {
                var resp = await _client.GetAsync($"Status/GetStatuses");
                var responce = resp.Content.ReadFromJsonAsync<IEnumerable<Status>>(_options);

                return responce.Result.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка связи, проверьте подключение к сети!", "Ошибка!");
                return [];
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                var resp = await _client.GetAsync($"User/GetUsers");
                var responce = resp.Content.ReadFromJsonAsync<IEnumerable<User>>(_options);

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

        public async Task<List<Role>> GetAllRoles()
        {
            try
            {
                var resp = await _client.GetAsync($"User/GetPosts");
                var responce = resp.Content.ReadFromJsonAsync<IEnumerable<Role>>(_options);

                var result = responce.Result.ToList();

                return result;
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

        public async Task DeletePpeForever(Ppe selectedItem)
        {
            if (selectedItem is null)
                return;

            try
            {
                var responce = await _client.DeleteAsync($"Ppes/DeletePpeForever?id={selectedItem.Id}");

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

        public async Task DeleteConditionForever(Model.Condition selectedItem)
        {
            if (selectedItem is null)
                return;

            try
            {
                var responce = await _client.DeleteAsync($"Conditions/DeleteConditionForever?id={selectedItem.Id}");

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

        public async Task DeletePostForever(Post selectedItem)
        {
            if (selectedItem is null)
                return;

            try
            {
                var responce = await _client.DeleteAsync($"Posts/DeletePostForever?id={selectedItem.Id}");

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

        public async Task DeletePpeTypeForever(PpeType selectedItem)
        {
            if (selectedItem is null)
                return;

            try
            {
                var responce = await _client.DeleteAsync($"PpeTypes/DeletePpeTypeForever?id={selectedItem.Id}");

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

        public async Task DeleteStatusForever(Status selectedItem)
        {
            if (selectedItem is null)
                return;

            try
            {
                var responce = await _client.DeleteAsync($"Status/DeleteStatusForever?id={selectedItem.Id}");

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

        public async Task DeletePersonForever(Person selectedItem)
        {
            if (selectedItem is null)
                return;

            try
            {
                var responce = await _client.DeleteAsync($"People/DeletePersonForever?id={selectedItem.Id}");

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

        public async Task DeleteUserForever(User selectedItem)
        {
            if (selectedItem is null)
                return;

            try
            {
                var responce = await _client.DeleteAsync($"User/DeleteUserForever?id={selectedItem.Id}");

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
                var responce = await _client.PostAsJsonAsync<Model.Condition>($"Conditions/PostCondition", item, _options);
                var list = await GetAllConditions();

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

        public async Task<bool> EditCondition(Model.Condition item)
        {
            var result = false;

            if (item is null)
                return result;

            try
            {
                var responce = await _client.PutAsJsonAsync($"Conditions/PutCondition", item, _options);
                var list = await GetAllConditions();

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
                var responce = await _client.PostAsJsonAsync<Post>($"Posts/PostPost", item, _options);
                var list = await GetAllPosts();

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
                var responce = await _client.PutAsJsonAsync<Post>($"Posts/PutPost", item, _options);
                var list = await GetAllPosts();

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
                var responce = await _client.PostAsJsonAsync<PpeType>($"PpeTypes/PostPpeType", item, _options);
                var list = await GetAllPpeTypes();

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
                var responce = await _client.PutAsJsonAsync<PpeType>($"PpeTypes/PutPpeType", item, _options);
                var list = await GetAllPpeTypes();

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
                var responce = await _client.PostAsJsonAsync<Status>($"Status/PostStatus", item, _options);
                var list = await GetAllStatuses();

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
                var responce = await _client.PutAsJsonAsync<Status>($"Status/PutStatus", item, _options);
                var list = await GetAllStatuses();

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
                var responce = await _client.PostAsJsonAsync($"User/PostUser", item, _options);
                var list = await GetAllUsers();

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

        public async Task<bool> AddRole(Role item)
        {
            var result = false;

            if (item is null)
                return result;

            try
            {
                var responce = await _client.PostAsJsonAsync($"User/PostRole", item, _options);
                var list = await GetAllRoles();

                if (responce is null)
                    return result;

                if (list.FirstOrDefault(s => s.Ttle == item.Ttle) is not null)
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
                var responce = await _client.PutAsJsonAsync($"User/PutUser", item, _options);
                var list = await GetAllUsers();

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
                var responce = await _client.PutAsJsonAsync($"Ppes/PutPpe", item, _options);
                var list = await GetAllPpes();

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
                var responce = await _client.PostAsJsonAsync<Ppe>($"Ppes/PostPpe", item, _options);
                var list = await GetAllPpes();

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
                var responce = await _client.PostAsJsonAsync($"People/PostPerson", item, _options);
                var list = await GetAllPersons();

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
                var responce = await _client.PutAsJsonAsync($"People/PutPerson", item, _options);
                var list = await GetAllPersons();

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