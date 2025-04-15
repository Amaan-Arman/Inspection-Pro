self.addEventListener('push', function (event) {
    const data = event.data.json();
    //const notiSound = new Audio('/admin_assets/sound/notification.wav');

    const options = {
        body: data.body,
        //icon: data.icon || '/path/to/default/icon.png',
        //icon: '/admin_assets/images/notification.png',
        badge: '/admin_assets/images/notification.png',
        data: { url: '/home' },
        sound: '/admin_assets/sound/notification.wav',
    };

    event.waitUntil(
        self.registration.showNotification(data.title, options)
    );
});

self.addEventListener('notificationclick', function (event) {
    event.notification.close();
    event.waitUntil(
        clients.openWindow(event.notification.data.url)
    );
});

//self.addEventListener('push', function (event) {
//    console.log('[Service Worker] Push Received.');
//    console.log(`[Service Worker] Push had this data: "${event.data.text()}"`);

//    const title = 'Push Codelab';
//    const options = {
//        body: 'Yay it works.',
//        icon: 'images/icon.png',
//        badge: 'images/badge.png'
//    };

//    event.waitUntil(self.registration.showNotification(title, options));
//});

