﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LodCoreLibrary.Infrastructure.Mailing {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class EventDescriptionResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal EventDescriptionResources() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LodCoreLibrary.Infrastructure.Mailing.EventDescriptionResources", typeof(EventDescriptionResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на К сожалению, {0} больше не работает над проектом {1}  :( 
        ///Проект всё равно будет завершен!.
        /// </summary>
        public static string DeveloperHasLeftProject {
            get {
                return ResourceManager.GetString("DeveloperHasLeftProject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на На сайте Лиги {0} оставил сообщение на тему {1}:
        ///{2}
        ///Обратный адрес - {3}.
        /// </summary>
        public static string NewContactMessage {
            get {
                return ResourceManager.GetString("NewContactMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на На проект {0} пришел новый разработчик! Встречайте - {1}! Теперь работа над проектом пойдет гораздо быстрее!.
        /// </summary>
        public static string NewDeveloperOnProject {
            get {
                return ResourceManager.GetString("NewDeveloperOnProject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} только что подтвердил свой электронный адрес ({1}) и ожидает окончательного подтверждения своего аккаунта от администратора! .
        /// </summary>
        public static string NewEmailConfirmedDeveloper {
            get {
                return ResourceManager.GetString("NewEmailConfirmedDeveloper", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Встречайте нового члена Лиги Разработчиков - {0}! Теперь {0} на полных правах один из нас!.
        /// </summary>
        public static string NewFullConfirmedDeveloper {
            get {
                return ResourceManager.GetString("NewFullConfirmedDeveloper", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Лига Разработчиков начала работу над новым проектом - {0}!
        ///{1}.
        /// </summary>
        public static string NewProjectCreated {
            get {
                return ResourceManager.GetString("NewProjectCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} для Лиги разработчиков оставлен новый заказ - {1}! За подробностями обращаться по адресу {2}.
        ///Описание заказа:
        ///{3}.
        /// </summary>
        public static string OrderPlaced {
            get {
                return ResourceManager.GetString("OrderPlaced", resourceCulture);
            }
        }
    }
}