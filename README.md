# LoginsAdmin (Xamarin)
Repositorio de LoginsAdmin para la versión para Android hecha con Xamarin.Forms

<img src="Screenshot.png" alt="LoginsAdmin | Administrador de credenciales - Hecho con ❤ por Jonatandb! (Versión C#/Xamarin.Forms - Android)"/>

### Páginas consultadas:

 - <a href="https://docs.microsoft.com/es-es/learn/modules/create-a-mobile-app-with-xamarin-forms/2-create-a-xf-project-in-vs" target="_blank">Creación de un proyecto de Xamarin.Forms en Visual Studio</a>

 - <a href="https://docs.microsoft.com/es-es/learn/modules/store-local-data-with-sqlite/" target="_blank">Almacenamiento de datos locales con SQLite en una aplicación de Xamarin.Forms</a>

 - <a href="https://docs.microsoft.com/es-es/xamarin/android/user-interface/splash-screen" target="_blank">Pantalla de presentación</a>
 
 - <a href="https://docs.microsoft.com/es-es/learn/modules/create-multi-page-xamarin-forms-apps-with-stack-and-tab-navigation/" target="_blank">Creación de aplicaciones de Xamarin.Forms de varias páginas con navegación de pila y pestaña</a>
 
 - <a href="https://docs.microsoft.com/es-es/samples/xamarin/xamarin-forms-samples/navigation-loginflow/" target="_blank">Xamarin.Forms - LoginFlow</a>

 - <a href="https://channel9.msdn.com/Series/Xamarin-101" target="_blank">Xamarin Tutorial 101</a>

 - <a href="https://docs.microsoft.com/es-es/xamarin/xamarin-forms/user-interface/searchbar" target="_blank">SearchBar de Xamarin.Forms</a>

 - <a href="https://stackoverflow.com/questions/54517874/how-to-make-a-floating-action-button-in-xamarin-forms" target="_blank">Ícono flotante (FAB: Floating action button)</a>

 - <a href="https://favicon.io/favicon-generator/" target="_blank">Favicon generator</a>

 - <a href="https://docs.microsoft.com/es-es/xamarin/xamarin-forms/user-interface/layouts/absolute-layout" target="_blank">Xamarin.Forms AbsoluteLayout</a>

 - <a href="https://docs.microsoft.com/es-es/xamarin/xamarin-forms/app-fundamentals/data-binding/commanding#using-command-parameters" target="_blank">La interfaz de comandos de Xamarin.Forms</a>

 - <a href="https://geeks.ms/etomas/2011/09/17/c-5-async-await/" target="_blank">C# 5: Async / Await</a>

 - <a href="https://stackoverflow.com/questions/46332349/xamarin-button-command-inside-of-listview-itemtemplate-not-firing" target="_blank">Xamarin Button Command (inside of ListView.ItemTemplate) Not Firing</a>


### Pendientes y "Nice to have":
	- Refactor de grilla para que al clickear un Servicio, permita editar y/o eliminar
		-	Ver modo de selección de la grilla
	- Refactorizar login para agregar ViewModel
	- Que no se pueda clickear en guardar si no se escribió un nombre de servicio
		- Usar CanExcecute() para habilitar boton de Guardar
	- Limitar maxlength al menos en el nombre de los servicios
	- Exportación/Importación de datos
	- Que se pueda configurar:
		- Que se pueda elegir que la busqueda respete mayusculas
		- Reestablecer scroll de la grilla al agregar un servicio
		- Si al estar creando un servicio se hace back y había algo escrito que se pida confirmación
		- Que se pida confirmación al eliminar un servicio
