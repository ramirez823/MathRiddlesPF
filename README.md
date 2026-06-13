¿Qué es Math Riddles Game?

Math Riddles Game es una aplicación web interactiva de acertijos matemáticos. Los jugadores se enfrentan a desafíos matemáticos de diferentes dificultades (opción múltiple o de texto libre) para ganar puntos y evolucionar su nivel de "IQ" (desde Principiante hasta Einstein). Durante el juego, los usuarios pueden utilizar power-ups estratégicos como eliminar opciones (50/50), pedir pistas o agregar tiempo extra al reloj. Todo esto ocurre de forma fluida y directa, ya que el sistema maneja las sesiones automáticamente sin obligar al usuario a registrarse o iniciar sesión.


¿Qué problema resuelve?

Este proyecto aborda dos necesidades principales, una desde la perspectiva del jugador
y otra muy importante desde la ingeniería de software:

    Para el usuario final: Elimina la fricción para empezar a jugar al no requerir 
    un proceso de login, ofreciendo un entorno rápido y entretenido para ejercitar la mente matemática.  

    A nivel técnico (Arquitectura): Resuelve el problema del código desorganizado o fuertemente acoplado.
    Al implementar una estricta arquitectura N-Tier (3 capas), el proyecto garantiza que la interfaz,
    las reglas del juego y la base de datos vivan de forma independiente. Esto resuelve los dolores 
    de cabeza de la escalabilidad: si el día de mañana se quiere cambiar la interfaz web actual por 
    una construida en React, o cambiar SQL Server por MongoDB, se puede hacer modificando solo la capa
    correspondiente sin romper el resto del sistema. Como lo define su propia arquitectura: 
    "Es como tener cajones organizados en lugar de todo revuelto en una bolsa".


Arquitectura del Proyecto

El sistema está dividido en tres capas principales:

    Capa de Presentación (FP.MVC): Es la interfaz con la que el usuario interactúa. Los Controllers reciben las peticiones del navegador, solicitan datos a los servicios y envían ViewModels a las Views para renderizar el HTML.  

    Capa de Lógica de Negocio (FP.CORE): Aquí residen las reglas del juego. Los Services se encargan de calcular puntuaciones, validar respuestas, actualizar el nivel de IQ y gestionar el uso de power-ups. Utiliza DTOs (Data Transfer Objects) para mover datos de forma segura y ligera entre capas.  

    Capa de Acceso a Datos (FP.DATA): Es la encargada de la comunicación directa con SQL Server. Utiliza Entities (tablas), Repositories para ejecutar consultas (SELECT, INSERT, UPDATE) y el DbContext como puente entre el código C# y la base de datos.  

 Características Principales

    Sesiones sin Login: Los jugadores son identificados automáticamente mediante un GUID único que se almacena en una cookie del navegador.  

    Sistema de Dificultad: Los acertijos se dividen en tres niveles: Principiante, Intermedio y Experto.  

    Tipos de Acertijos: Incluye preguntas de opción múltiple (cuatro opciones) y preguntas de texto libre.  

    Progresión (Niveles de IQ): Cuenta con siete niveles de inteligencia que el jugador alcanza según sus puntos acumulados, desde Principiante hasta Einstein.  

    Power-ups: Los jugadores disponen de tres ventajas tácticas: 50/50 (elimina dos opciones incorrectas), Hint (muestra una pista) y ExtraTime (añade quince segundos al cronómetro).  

    Puntuación Dinámica: Otorga puntos base al acertar y un bonus por rapidez proporcional al tiempo ahorrado.  

 Tecnologías Utilizadas

    Backend: C#.  

    Framework Web: ASP.NET MVC.  

    Base de Datos: SQL Server gestionado a través de Entity Framework.  

    Inyección de Dependencias: Autofac (Versión 5.2.0 para evitar conflictos con .NET Framework 4.8.1).  

    Serialización: Newtonsoft.Json (Versión 13.0.3).  

    Frontend: HTML, Bootstrap y JavaScript, con un diseño visual basado en estilos neón/cyberpunk.
