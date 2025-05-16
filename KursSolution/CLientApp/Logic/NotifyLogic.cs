using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CLientApp.Model;

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

        private async Task GetPpes()
            => _ppes = await _db.GetAllPpes();
        private void CreateNotifications()
        {
            List<Notify_Model> result = [];

            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Не удалось получить уведомления!");
            }

            _notifications = result;
        }

        public List<Notify_Model> GetNotifyes()
            => _notifications;
    }
}
