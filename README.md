# TaxiApp
Мини приложение такси

бизнес логика: 
1) пользователи заказывают такси с личного аккаунта, который до этого зарегестрировали.(имя/пароль)
2) оставляют заявку из точки А (без заморочек - enum на несколько точек)
3) водителям через соккеты приходит оповещение о заказе (водители тоже зарегистрированны)
4) водитель нажимет кнопку принять
5) запускается таймер на 5 минут, по истечению которого пользователю приходит оповещение что машина подъехала.

Стэк: WPF, Entity Framework, signalR, postgresSQL, .net 3,1