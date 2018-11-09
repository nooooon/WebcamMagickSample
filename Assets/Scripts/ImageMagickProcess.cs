using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImageMagick;

public class ImageMagickProcess {
    
    public ImageMagickProcess() {


    }
    
    public MagickImage Comvert(byte[] img, string path) {

        try {
            using(MagickImage mi1 = new MagickImage(img)) {

                using(MagickImage mi2 = new MagickImage(path)) {

                    mi1.Composite(mi2, 0, 0, CompositeOperator.Over);

                    return new MagickImage(mi1);
                }
            }
        } catch(System.Exception e) {
            Debug.LogError(e);

            return null;
        }
    }

    public void Save(MagickImage mi, string savePath, string fileType) {
        mi.Write(savePath + "." + fileType);
    }
}
