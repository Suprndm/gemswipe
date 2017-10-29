using System;

namespace GemSwipe.Paladin.UIElements.Popups
{
    public class PopupService
    {
        private static PopupService _instance;
        public event Action<IDialogPopup> OnNewPopup;

        private PopupService()
        {
        }

        public void ShowPopup(IDialogPopup dialogPopup)
        {
            OnNewPopup?.Invoke(dialogPopup);
            dialogPopup.Show();
        }

        public static PopupService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PopupService();
                }
                return _instance;
            }
        }

    }
}
