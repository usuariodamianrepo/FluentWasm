# Backend
	
## Revisar estas dos clases valen la pena tenerlas usando FluentUI
en le anterior poryecto había un módulo desarrollado para las notificaciones
builder.Services.AddScoped<IAuthenticationStateNotifier, AuthenticationStateNotifier>();
builder.Services.AddScoped<IAuthenticatedClientStateCleaner, AuthenticatedClientStateCleaner>();

AuthenticatedClientStateCleaner: eliminado en las clases relacionadas;

¿para que sirve NotificationService ?
NotificationService sirve para centralizar las notificaciones del frontend: mensajes, inbox, contadores, lectura, limpieza, etc.
En tu proyecto, se usa indirectamente en la autenticación:
•	AuthenticatedClientStateCleaner depende de INotificationService
•	cuando el token expira o falla el refresh, se llama a:
•	ClearInbox()
O sea, la idea es que cuando el usuario cierra sesión o se invalida la sesión, también se limpien las notificaciones del cliente.
En tu código actual, la implementación es mínima porque solo existe como placeholder:
•	UnreadCount => 0
•	ClearInbox() no hace nada
Eso está bien si todavía no tenés una funcionalidad real de notificaciones, pero en una app completa normalmente este servicio debería:
•	mantener la lista de notificaciones
•	generar eventos cuando cambian
•	marcar como leídas
•	limpiar inbox
•	exponer UnreadCount
En resumen:
•	NotificationService no es de login en sí
•	es un servicio de UI/estado para notificaciones
•	y en este caso lo usan para limpiar el estado del usuario cuando se invalida la sesión