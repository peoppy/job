using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using Sklad.Module.BusinessObjects.TEST;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Sklad.Module.Controllers
{
    public partial class CustomModificationControllerPlatform : ViewController
    {
        /// <summary> Коллекция площадок </summary>
        IList<Platform> platforms;

        /// <summary> Коллекция элементов таблицы "Изменения груза на площадках" </summary>
        IList<CargoChangeHistory> cargoChangeHistories;

        /// <summary> Коллекция элементов таблицы "Изменения состава склада" </summary>
        IList<PlatformChangeHistory> platformChangeHistories;

        /// <summary> Базовый контроллер, реализующий действия: сохранить, сохранить и закрыть, сохранить и создать новый </summary>
        ModificationsController controller;

        /// <summary> Сохраняемая площадка </summary>
        Platform platformSave;

        /// <summary> Площадка до сохранения </summary>
        Platform platformOld;

        /// <summary> Элемент таблицы "Изменения груза на площадках" </summary>
        CargoChangeHistory cargoChangeHistory;

        /// <summary> Элемент таблицы "Изменения состава склада" </summary>
        PlatformChangeHistory platformChangeHistory;

        /// <summary> Последняя запись с информацией о площадке до сохранения (Без даты по) </summary>
        CargoChangeHistory cargoChange;

        /// <summary> Последняя запись с информацией о площадке до сохранения (Без даты по) </summary>
        PlatformChangeHistory platformChange;


        public CustomModificationControllerPlatform()
        {
            InitializeComponent();
            this.TargetObjectType = typeof(Platform); //Тип целевого объекта (Площадки)
            this.TargetViewType = ViewType.DetailView; //Тип целевого представления (Подробный вид)
        }

        //Событие при активации контроллера
        protected override void OnActivated()
        {
            base.OnActivated();
            controller = Frame.GetController<ModificationsController>();

            if (controller != null)
            {      
                //Получение коллекции площадок (коллекция до изменений)
                IObjectSpace objectSpacePlatform = Application.CreateObjectSpace(typeof(Platform));
                platforms = objectSpacePlatform.GetObjects<Platform>();

                cargoChangeHistories = View.ObjectSpace.GetObjects<CargoChangeHistory>();

                platformChangeHistories = View.ObjectSpace.GetObjects<PlatformChangeHistory>();

                //Переопределение обработки вызова события "Сохранить"
                controller.SaveAction.Executing += SaveExecuting;

                //Переопределение обработки вызова события "Сохранить и закрыть"
                controller.SaveAndCloseAction.Executing += SaveExecuting;

                //Переопределение обработки вызова события "Сохранить и создать"
                controller.SaveAndNewAction.Executing += SaveExecuting;
            }
        }
        
        private void SaveExecuting(object sender, CancelEventArgs e)
        {
            platformSave = (Platform)View.CurrentObject;
            
            platformOld = platforms.FirstOrDefault(platform => platform.ID == platformSave.ID);

            if (platformOld == null){

                cargoChangeHistory = new CargoChangeHistory(platformSave.Session);
                cargoChangeHistory.namePlatform = platformSave.Name; //Запись названия площадки
                cargoChangeHistory.Cargo = platformSave.cargo; //Запись веса груза
                cargoChangeHistory.DateSince = DateTime.Now; //Запить даты с
                cargoChangeHistories.Add(cargoChangeHistory); //Добавление новой зиписи в коллекцию
                View.ObjectSpace.SetModified(cargoChangeHistories); 

                platformChangeHistory = new PlatformChangeHistory(platformSave.Session);
                platformChangeHistory.namePlatform = platformSave.Name; //Запись названия площадки
                platformChangeHistory.nameSklad = platformSave.IDsklad.Name; //Запись названия склада
                platformChangeHistory.DateSince = DateTime.Now; //Запись даты с
                platformChangeHistories.Add(platformChangeHistory);//Добавление новой зиписи в коллекцию
                View.ObjectSpace.SetModified(platformChangeHistories);
                return;
            }

            //Если вес груза отличается, то записываем изменения в таблицу
            if (platformSave.cargo != platformOld.cargo)
            {
                //Находим последнюю запись с информацией о площадке до сохранения (Без даты по)
                cargoChange = cargoChangeHistories.FirstOrDefault(item => item.namePlatform == platformOld.Name &&
                item.Cargo == platformOld.cargo && item.DateBy == default);
                //Запись даты изменения 
                cargoChange.DateBy = DateTime.Now;
                //Сохранение изменений последней записи
                View.ObjectSpace.SetModified(cargoChange);

                //Создание новой записи
                CargoChangeHistory newChangeHistory = new CargoChangeHistory(cargoChange.Session);
                newChangeHistory.namePlatform = platformSave.Name; //Запись названия
                newChangeHistory.Cargo = platformSave.cargo; //Запись веса груза
                newChangeHistory.DateSince = DateTime.Now; //Запись даты с
                //Добавление новой записи в список
                cargoChangeHistories.Add(newChangeHistory);
                //Сохранение изменений списка
                View.ObjectSpace.SetModified(cargoChangeHistories);
            }

            if (platformSave.IDsklad != null && platformOld.IDsklad != null)
            {
                //Если название склада отличается, то записываем изменения в таблицу
                if (platformSave.IDsklad.ID != platformOld.IDsklad.ID)
                {
                    //Находим последнюю запись (Без даты по)
                    platformChange = platformChangeHistories.FirstOrDefault(item => item.namePlatform == platformOld.Name &&
                    item.nameSklad == platformOld.IDsklad.Name && item.DateBy == default);
                    //Запись времени
                    platformChange.DateBy = DateTime.Now;
                    //Сохранение изменений последней записи
                    View.ObjectSpace.SetModified(platformChange);

                    //Создание новой записи
                    PlatformChangeHistory newChangeHistory = new PlatformChangeHistory(platformChange.Session);
                    newChangeHistory.namePlatform = platformSave.Name; //Запись названия
                    newChangeHistory.nameSklad = platformSave.IDsklad.Name; //Запись вес груза
                    newChangeHistory.DateSince = DateTime.Now; //Запись даты с
                                                               //Добавление новой записи в список
                    platformChangeHistories.Add(newChangeHistory);
                    //Сохранение изменений списка
                    View.ObjectSpace.SetModified(platformChangeHistories);
                }
            }
        }

        protected override void OnDeactivated()
        {
            //Метод обновления всех объектов
            View.ObjectSpace.CommitChanges();              

            base.OnDeactivated();
        }
    }
}
