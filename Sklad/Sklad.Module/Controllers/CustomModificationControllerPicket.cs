using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using Sklad.Module.BusinessObjects.TEST;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Sklad.Module.Controllers
{
    public partial class CustomModificationControllerPicket : ViewController
    {
        /// <summary> Коллекция пикетов </summary>
        IList<Picket> pickets;

        /// <summary> Базовый контроллер, реализующий действия: сохранить, сохранить и закрыть, сохранить и создать новый </summary>
        ModificationsController controllerMod;

        /// <summary> Базовый контроллер, реализующий действие "Удаление" </summary>
        DeleteObjectsViewController controllerDel;

        /// <summary> Удаляемый пикет </summary>
        Picket picketDel;

        /// <summary> Пикет перед удаляемым (Number - 1) </summary>
        Picket picketBeforeDel;

        /// <summary> Пикет после удаляемого (Number + 1) </summary>
        Picket picketAfterDel;

        /// <summary> Сохраняемый пикет </summary>
        Picket picketSave;

        /// <summary> Пикет до сохранения </summary>
        Picket picketOld;

        /// <summary> Пикет перед сохраняемым (Number - 1) </summary>
        Picket picketBeforeSave = null;

        /// <summary> Пикет после сохраняемого (Number + 1) </summary>
        Picket picketAfterSave = null;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public CustomModificationControllerPicket()
        {
            InitializeComponent();
            this.TargetObjectType = typeof(Picket); //Тип целевого объекта (Пикеты)
            this.TargetViewType = ViewType.DetailView; //Тип целевого представления (Подробный вид)
        }

        /// <summary>
        /// Событие при активации контроллера
        /// </summary>
        protected override void OnActivated()
        {
            base.OnActivated();
            
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Picket));
            pickets = objectSpace.GetObjects<Picket>();

            controllerMod = Frame.GetController<ModificationsController>();

            if (controllerMod != null)
            {     
                //Переопределение обработки вызова события "Сохранить"
                controllerMod.SaveAction.Executing += SaveExecuting;

                //Переопределение обработки вызова события "Сохранить и закрыть"
                controllerMod.SaveAndCloseAction.Executing += SaveExecuting;

                //Переопределение обработки вызова события "Сохранить и создать"
                controllerMod.SaveAndNewAction.Executing += SaveExecuting;
            }

            controllerDel = Frame.GetController<DeleteObjectsViewController>();

            if (controllerDel != null)
            {
                //Переопределение обработки вызова события "Удалить"
                controllerDel.DeleteAction.Executing += DeleteExecuting;
            }
        }

        /// <summary>
        /// Событие сделано для выполнения проверки удаляемого пикета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteExecuting(object sender, CancelEventArgs e)
        {
            picketDel = ((Picket)View.CurrentObject);

            picketBeforeDel = pickets.FirstOrDefault(picket => picket.Number == picketDel.Number - 1);

            picketAfterDel = pickets.FirstOrDefault(picket => picket.Number == picketDel.Number + 1);

            //Если нет хотябы одного из соседних пикетов, то можно удалить
            if (picketBeforeDel != null && picketAfterDel != null)
            {
                //Если у соседних пикетов равна платформа, то пикет нельзя удалять
                if (picketBeforeDel.IDplatform == picketAfterDel.IDplatform)
                {
                    Application.ShowViewStrategy.ShowMessage("Error delete Picket", InformationType.Error, 1000, InformationPosition.Top);
                    e.Cancel = true;
                    return;
                }
            }
            View.ObjectSpace.SetModified(picketDel);
        }

        /// <summary>
        /// Событие сделано для выполнения проверки сохраняемого пикета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveExecuting(object sender, CancelEventArgs e)
        {
            picketSave = ((Picket)View.CurrentObject);
            
            picketOld = pickets.FirstOrDefault(picket => picket.ID == picketSave.ID);

            //Если пикет не менялся, то сохраняем
            if (picketOld != null)
            {
                if (picketSave.IDplatform != null && picketOld.IDplatform != null)
                {
                    if (picketSave.IDplatform.ID == picketOld.IDplatform.ID &&
                        picketSave.Number == picketOld.Number)
                    {
                        e.Cancel = false;
                        return;
                    }
                }

                picketBeforeSave = pickets.FirstOrDefault(picket => picket.Number == picketOld.Number - 1);

                picketAfterSave = pickets.FirstOrDefault(picket => picket.Number == picketOld.Number + 1);
            }            
            
            //Если нет хотябы одного из соседних пикетов, то можно переносить
            if (picketBeforeSave != null && picketAfterSave != null)
            {
                //Если у соседних пикетов равна платформа, то сохраняемый пикет нельзя переносить
                if (picketBeforeSave.IDplatform == picketAfterSave.IDplatform)
                {
                    Application.ShowViewStrategy.ShowMessage("Error save Picket", InformationType.Error, 1000, InformationPosition.Top);
                    e.Cancel = true;
                    return;
                }
            }            

            //Если у сохраняемого пикета указана платформа
            if (picketSave.IDplatform != null)
            {
                if (picketSave.IDplatform.Pickets.Count > 1) //У платформы всегда будет хотябы один пикет (сохраняемый)
                {
                    //Проверка может ли сохраняемый пикет стоять в начале списка 
                    if (picketSave.Number - picketSave.IDplatform.Pickets.First().Number == -1)
                    {
                        e.Cancel = false;
                        return;
                    }
                    else if (picketSave.Number - picketSave.IDplatform.Pickets[picketSave.IDplatform.Pickets.Count - 2].Number == 1)
                    {
                        //Проверка может ли сохраняемый пикет стоять в конце списка 
                        //(Т.к. пикет заранее вставляется в конец списка, то необходимо сравнивать с предпоследним)
                        e.Cancel = false;
                        return;
                    }
                    else
                    {
                        //Если пикет не может стоять не в начале и не в конце, то перенести его на эту платформу нельзя
                        Application.ShowViewStrategy.ShowMessage("Error save Picket", InformationType.Error, 1000, InformationPosition.Top);
                        e.Cancel = true;
                        return;
                    }                    
                }
                else
                {
                    //Если у платформы нет пикетов, то сохраняемый пикет можно перенести
                   e.Cancel = false;
                   return;
                }                
            }
            else //Если не указана платформа, то пикет можно сохранить не учтённым (без платформы)
            {
                e.Cancel = false;
                return;
            }
        }

        /// <summary>
        /// Событие при деактивации контроллера
        /// </summary>
        protected override void OnDeactivated()
        {
            //Метод обновления всех объектов
            View.ObjectSpace.CommitChanges();

            base.OnDeactivated();
        }
    }
}
