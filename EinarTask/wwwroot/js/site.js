
// Sayfa yüklendiğinde 1.5 saniye sonra loader kaybolsun

loadingAnimate();

function loadingAnimate(){
    window.addEventListener('load', () => {
        setTimeout(() => {
            document.getElementById('loader').classList.add('fade-out');
            setTimeout(() => {
                document.getElementById('loader').style.display = 'none';
                document.getElementById('main-content').style.display = 'block';
            }, 500); // fade-out süresi
        }, 1500); // 1.5 saniye gösterim süresi
    });

}