using System;
using System.Collections.Generic;
using System.Text;

namespace task_10.services
{
    /// Сервис диалоговых окон.
    /// Абстракция для вывода сообщений
    public interface IDialogService
    {
        void ShowInfo(string message);
        void ShowWarning(string message);
        void ShowError(string message);
        bool ShowConfirmation(string message);
    }
}
