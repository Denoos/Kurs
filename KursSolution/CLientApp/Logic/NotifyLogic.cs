using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using CLientApp.Model;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace CLientApp.Logic
{
    public class NotifyLogic
    {
        private static NotifyLogic instance;
        public static NotifyLogic Instance { get => instance ??= new(); }

        private readonly DataBaseEndPoint _db = DataBaseEndPoint.Instance;
        private List<Notify_Model> _notifications;
        private List<Ppe> _ppes;

        public NotifyLogic()
        {
            GetPpes();
            CreateNotifications();
        }
        
        public NotifyLogic(List<Ppe> ppes)
        {
            _ppes = ppes;
            CreateNotifications();
        }

        private async Task GetPpes()
        { _ppes = await _db.GetAllPpes(); }

        private void CreateNotifications()
        {
            List<Notify_Model> result = [];

            try
            {
                var index = 0;
                var title = "";
                var content = "";
                var now = DateTime.Now.Date;
                if (_ppes is not null)
                    foreach (var ppe in _ppes)
                    {
                        title = ppe.Title;
                        var end = new DateTime(ppe.DateEnd, TimeOnly.MinValue);

                        if (now >= end)
                        {
                            content = $"{ppe.Title} с инвернтарным номером {ppe.InventoryNumber} будет просрочен сегодня, или просрочен уже! (дата истечения срока: {ppe.DateEnd})";
                            result.Add(new Notify_Model { Id = index, Content = content, Title = title });

                            index++;
                            continue;
                        }

                        var delta = end - now;

                        if (delta.TotalDays <= 90)
                            content = $"{ppe.Title} с инвернтарным номером {ppe.InventoryNumber} будет просрочен через {delta.TotalDays.ToString()} дней. Стоит обратить внимание на это СИЗ! (дата истечения срока: {ppe.DateEnd}).";
                        else if (delta.TotalDays <= 30)
                            content = $"Внимание, {ppe.Title} с инвернтарным номером {ppe.InventoryNumber} будет просрочен через {delta.TotalDays.ToString()} дней! Рекомендуется заказать замену. (дата истечения срока: {ppe.DateEnd})!";
                        else if (delta.TotalDays <= 7)
                            content = $"ВНИМАНИЕ! {ppe.Title} С НОМЕРОМ {ppe.InventoryNumber} БУДЕТ ИСПОРЧЕН В ТЕЧЕНИИ НЕДЕЛИ!!! ЗАМЕНА НЕОБХОДИМА НЕМЕДЛЕННО!!!! ОСТАЛОСЬ ДНЕЙ: {delta.TotalDays.ToString().ToUpper()}!!! ДАТА ИСТЕЧЕНИЯ СРОКА: {ppe.DateEnd})!!!!!!!";

                        result.Add(new Notify_Model { Id = index, Content = content, Title = title });
                        index++;
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Не удалось получить уведомления!");
            }

            _notifications = result;
        }

        public List<Notify_Model> GetNotifyes()
            => _notifications;

        public int HetCountNotify()
        {
            if (_notifications == null || _notifications.Count == 0)
                CreateNotifications();
            return _notifications.Count();
        }
    }
}
