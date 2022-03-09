using DevExpress.ExpressApp;
using Sklad.Module.BusinessObjects;
using System.Collections.Generic;
using System.Linq;

namespace Sklad.Module.Controllers
{
    public partial class ViewControllerCustomCommitting : ViewController
    {
        /// <summary> Коллекция пикетов </summary>
        IList<Picket> pickets;
        /// <summary> Коллекция площадок </summary>
        IList<Platform> platforms;
        public ViewControllerCustomCommitting()
        {
            InitializeComponent();
        }
           
        protected override void OnActivated()
        {
            base.OnActivated();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Picket));
            pickets = objectSpace.GetObjects<Picket>();
            platforms = objectSpace.GetObjects<Platform>();

            ObjectSpace.Committing += ObjectSpace_Committing;
        }

        private void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IObjectSpace os = (IObjectSpace)sender;

            for (int i = os.ModifiedObjects.Count - 1; i >= 0; i--)
            {
                object item = os.ModifiedObjects[i];

                //Определение действия над объектом (создание, изменение, удаление)
                if (os.IsNewObject(item))
                {
                    //Проверка создания площадки
                    if (typeof(Platform).IsAssignableFrom(item.GetType()))
                    {
                        Platform AddPlatform = (Platform)item;
                        //Проверка на отрицательный груз
                        if (AddPlatform.CargoWeight < 0)
                        {
                            Application.ShowViewStrategy.ShowMessage("Вес груза не может быть отрицательным!", InformationType.Error, 1000, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                        else if (AddPlatform.CargoWeight > 0 && !AddPlatform.IsFormed)
                        {
                            Application.ShowViewStrategy.ShowMessage("Площадка не может быть с грузом и не сформирована!", InformationType.Error, 1000, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                else if (os.IsObjectToSave(item))
                {
                    //Проверка изменения площадки
                    if (typeof(Platform).IsAssignableFrom(item.GetType()))
                    {
                        Platform CommittingPlatform = (Platform)item;
                        //Проверка на отрицательный груз
                        if (CommittingPlatform.CargoWeight < 0)
                        {
                            Application.ShowViewStrategy.ShowMessage("Вес груза не может быть отрицательным!", InformationType.Error, 1000, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }

                        //Проверка есть ли груз для расформирования площадки
                        if (!CommittingPlatform.IsFormed && CommittingPlatform.CargoWeight != 0)
                        {
                            Application.ShowViewStrategy.ShowMessage("Запрещено расформировывать площадку! На площадке имеется груз!", InformationType.Error, 1000, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                    }
                    //Проверка изменения пикета
                    else if (typeof(Picket).IsAssignableFrom(item.GetType()))
                    {
                        Picket CommittingPicket = (Picket)item;
                        Picket OldPicket = pickets.FirstOrDefault(picket => picket.Oid == CommittingPicket.Oid);

                        //Если площадки не одинаковые, то проверяется возможность перемещения
                        if (OldPicket.IDplatform != CommittingPicket.IDplatform)
                        {
                            //Проверка площадки до обновления
                            if (OldPicket.IDplatform != null)
                            {
                                //Если площадка сформирована, то запрещено перемещать пикеты
                                if (OldPicket.IDplatform.IsFormed)
                                {
                                    Application.ShowViewStrategy.ShowMessage("Запрещено перемещение пикетов из сформированной площадки!", InformationType.Error, 1000, InformationPosition.Top);
                                    e.Cancel = true;
                                    return;
                                }
                                else
                                {
                                    //Если есть площадки перед и после пикета, то перемещение невозможно
                                    if (OldPicket.IDplatform.Pickets.FirstOrDefault(picket => picket.Number == CommittingPicket.Number - 1) != null &&
                                    OldPicket.IDplatform.Pickets.FirstOrDefault(picket => picket.Number == CommittingPicket.Number + 1) != null)
                                    {
                                        Application.ShowViewStrategy.ShowMessage("Запрещено перемещение пикетов из середины площадки!", InformationType.Error, 1000, InformationPosition.Top);
                                        e.Cancel = true;
                                        return;
                                    }
                                }
                            }

                            //Проверка площадки сохраняемого пикета
                            if (CommittingPicket.IDplatform != null)
                            {
                                if (CommittingPicket.IDplatform.IsFormed)
                                {
                                    Application.ShowViewStrategy.ShowMessage("Запрещено перемещение пикетов в сформированную площадку!", InformationType.Error, 1000, InformationPosition.Top);
                                    e.Cancel = true;
                                    return;
                                }
                                else if (CommittingPicket.IDplatform.Pickets.Count > 1)
                                {
                                    //Если пикет не может стоять в начале или в конце площадки, то перемещение невозможно
                                    if (CommittingPicket.IDplatform.Pickets.FirstOrDefault(picket => picket.Number - 1 == CommittingPicket.Number || picket.Number + 1 == CommittingPicket.Number) == null)
                                    {
                                        Application.ShowViewStrategy.ShowMessage("Пикет с текущим номером не возможно перенести на выбранную площадку!", InformationType.Error, 1000, InformationPosition.Top);
                                        e.Cancel = true;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (os.IsObjectToDelete(item))
                {
                    //Проверка удаления склада
                    if (typeof(Warehouse).IsAssignableFrom(item.GetType()))
                    {
                        Warehouse DeleteWarehouse = (Warehouse)item;
                        //Проверяется коллекция площадок на сформированность
                        if (DeleteWarehouse.Platforms != null && DeleteWarehouse.Platforms.FirstOrDefault(warehouse => warehouse.IsFormed) != null)
                        {
                            Application.ShowViewStrategy.ShowMessage("Запрещено удалять склад, если он имеет в составе сформированные площадки!", InformationType.Error, 10000, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                    }
                    //Проверка удаления площадки
                    else if (typeof(Platform).IsAssignableFrom(item.GetType()))
                    {
                        Platform DeletePlatform = (Platform)item;
                        //Проверка статуса формирования площадки 
                        if (DeletePlatform.IsFormed)
                        {
                            Application.ShowViewStrategy.ShowMessage("Невозможно удалить сформированную площадку!", InformationType.Error, 10000, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                    }
                    //Проверка удаления пикета
                    else if (typeof(Picket).IsAssignableFrom(item.GetType()))
                    {
                        Picket DeletePicket = (Picket)item;
                        //Проверка площадки удаляемого пикета на статус формирования
                        if (DeletePicket.IDplatform != null && DeletePicket.IDplatform.IsFormed)
                        {
                            Application.ShowViewStrategy.ShowMessage("Невозможно удалить пикет из формированной площадки!", InformationType.Error, 10000, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                    }
                    //Проерка удаления типа груза
                    else if (typeof(Cargo).IsAssignableFrom(item.GetType()))
                    {
                        Cargo DeleteCargo = (Cargo)item;
                        if (DeleteCargo.Platforms != null && DeleteCargo.Platforms.FirstOrDefault(cargo => cargo.IsFormed) != null)
                        {
                            Application.ShowViewStrategy.ShowMessage("Невозможно удалить тип груза! Имеются формированные площадки с этим типом груза.", InformationType.Error, 10000, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
        }

        protected override void OnDeactivated()
        {
            ObjectSpace.Committing -= ObjectSpace_Committing;
            base.OnDeactivated();
        }
    }
}
