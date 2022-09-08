var resize;
var base64data;

window.Crop = {
    image: function (component) {

        setTimeout(() => {

            var cor = document.getElementById('upload-demo');
            resize = new Croppie(cor, {
                enableExif: true,
                viewport: {
                    width: 150,
                    height: 150,
                    type: 'square'
                },
                boundary: {
                    width: 300,
                    height: 300
                }
            });

            var input = document.getElementById('upload').files[0];
            if (input) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    //document.getElementsByClassName('upload-demo').classList.add('ready');
                    resize.bind({
                        url: e.target.result
                    });
                }
                reader.readAsDataURL(input);
            }
            else {
                alert("Sorry - you're browser doesn't support the FileReader API");
            }
        }, 150);

    },
    responses: function (component) {
        resize.result('blob').then(function (blob) {
            var reader = new FileReader();
            reader.readAsDataURL(blob);
            reader.onloadend = async function () {
                base64data = reader.result;
                var i = document.getElementById('result');
                var img = document.createElement('img');
                img.src = base64data;                
                i.appendChild(img);
                return component.invokeMethodAsync('ResponseMethod',base64data); 
            }
        });
        
    }
};

   