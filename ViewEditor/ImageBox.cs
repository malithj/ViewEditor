using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewEditor
{
    public class ImageBox
    {
        private Hashtable hashTable
        {
            get;
            set;
        }
        /** function filters RGB color schemes */
        public Bitmap[] filterRGB(string fileNameLoad)
        {
            //test file name path 
            string fileName = fileNameLoad;

            try
            {
                //load the image 
                Bitmap bmp = new Bitmap(fileName);



                int height = bmp.Height;
                int width = bmp.Width;

                //processing bitmaps
                Bitmap redMap = new Bitmap(fileName);
                Bitmap greenMap = new Bitmap(fileName);
                Bitmap blueMap = new Bitmap(fileName);

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Color pixelColor = bmp.GetPixel(j, i);

                        //seperate channels
                        int alpha = pixelColor.A;
                        int redChannel = pixelColor.R;
                        int blueChannel = pixelColor.B;
                        int greenChannel = pixelColor.G;

                        //filtering the images
                        redMap.SetPixel(j, i, Color.FromArgb(alpha, redChannel, 0, 0));
                        greenMap.SetPixel(j, i, Color.FromArgb(alpha, 0, greenChannel, 0));
                        blueMap.SetPixel(j, i, Color.FromArgb(alpha, 0, 0, blueChannel));
                    }
                }

                //load seperate bitmaps to return from the model object 
                Bitmap[] map = new Bitmap[3];
                map[0] = redMap;
                map[1] = greenMap;
                map[2] = blueMap;

                return map;


            }
            catch (Exception Err)
            {
                Console.WriteLine(Err);
                return null;
            }
        }

        /** gray scale the image */
        public Bitmap rgbToGray(Bitmap bitmap)
        {
            int height = bitmap.Height;
            int width = bitmap.Width;
            Bitmap result = new Bitmap(bitmap);

            for (int i = 0; i < width;i++){
                for(int j=0;j<height;j++){
                    Color color = bitmap.GetPixel(i, j);
                    int grayscaleValue = (int)(0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B);
                    Color newColor = Color.FromArgb(grayscaleValue,grayscaleValue,grayscaleValue);
                    result.SetPixel(i, j, newColor);
                }
            }
            return result;
        }

        /** Create histogram */
        public Bitmap importHistogram(Bitmap bitmap)
        {
            Bitmap grayImage = rgbToGray(bitmap);
            int histHeight = 128; 
            Bitmap result = new Bitmap(256, histHeight+10);
            int[] histogram = new int[256];
            float max = 0;

            for (int i = 0; i < grayImage.Width; i++)
            {
                for (int j = 0; j < grayImage.Height; j++)
                {
                    int intensity = grayImage.GetPixel(i, j).R;
                    histogram[intensity]++;
                    if (max < histogram[intensity])
                    {
                        max = histogram[intensity];
                    }
                }
            }

            using (Graphics g = Graphics.FromImage(result))
            {
                Pen myPen = new Pen(new SolidBrush(Color.Black), 10);
                for (int i = 0; i < histogram.Length; i++)
                {
                    float pct = histogram[i] / max;   // Precentage of max Value
                    g.DrawLine(Pens.Black,
                        new Point(i, result.Height),  
                        new Point(i, result.Height- (int)(pct * histHeight))  //Percentage of the height
                        );
                }
                //draw rectangle
                g.DrawRectangle(new System.Drawing.Pen(new SolidBrush(Color.Black), 1), 0, 0,result.Width - 1, result.Height - 1);
            }
                return result;
        }

        /** Change brightness */
        public Bitmap changeBrightness(Bitmap bitmap, int increment)
        {
            Bitmap bmp = bitmap;
            Bitmap resultbmp = new Bitmap(bitmap, bitmap.Size);

            int height = bmp.Height;
            int width = bmp.Width;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //Blue Color
                    int blue = truncate((bmp.GetPixel(i, j).B + increment));
                    //Red Color
                    int red = truncate((bmp.GetPixel(i, j).R + increment));
                    //Green Color 
                    int green = truncate((bmp.GetPixel(i, j).G + increment));

                    resultbmp.SetPixel(i, j, Color.FromArgb(red, green, blue));
                }
            }
            return resultbmp;
        }

        /** Checks for the range 0f 0-255 */
        public int truncate(int byteCol)
        {
            if ((byteCol) > 255)
            {
                return (255);
            }
            else if ((byteCol) < 0)
            {

                return (0);
            }
            else
            {
                return byteCol;
            }
        }

        /** Invert Colours of the image */
        public Bitmap invertColors(Bitmap bitmap)
        {
            Bitmap bitmapResult = new Bitmap(bitmap, bitmap.Size);
            Bitmap bmp = bitmap;


            int height = bmp.Height;
            int width = bmp.Width;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //Blue Color
                    int blue = 255 - bmp.GetPixel(i, j).B;
                    //Red Color
                    int red = 255 - bmp.GetPixel(i, j).R;
                    //Green Color 
                    int green = 255 - bmp.GetPixel(i, j).G;

                    bitmapResult.SetPixel(i, j, Color.FromArgb(red, green, blue));
                }
            } 

            return bitmapResult;
        }

        /** Contrast Adjustment of the image */
        public Bitmap contrastChange(Bitmap bitmap, double contrast)
        {
            Bitmap bitmapResult = new Bitmap(bitmap, bitmap.Size);
            Bitmap bmp = bitmap;

            double factor;
            factor = contrast > 0 ? 0.09 * contrast : 0.01 * (contrast + 100);

            int height = bmp.Height;
            int width = bmp.Width;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //Blue Color
                    int blue = truncate((int)Math.Round(factor * (bmp.GetPixel(i, j).B - 128) + 128));
                    //Red Color
                    int red = truncate((int)Math.Round(factor * (bmp.GetPixel(i, j).R - 128) + 128));
                    //Green Color 
                    int green = truncate((int)Math.Round(factor * (bmp.GetPixel(i, j).G - 128) + 128));

                    bitmapResult.SetPixel(i, j, Color.FromArgb(red, green, blue));
                }
            }

            return bitmapResult;
        }

        /** Vertical flip the image */
        public Bitmap verticalFlip(Bitmap bitmap)
        {
            Bitmap result = new Bitmap(bitmap, bitmap.Size);
            int height = bitmap.Height;
            int width = bitmap.Width; 

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    result.SetPixel(width-1-i, j, Color.FromArgb(bitmap.GetPixel(i, j).R, bitmap.GetPixel(i, j).G, bitmap.GetPixel(i, j).B));
                }
            }
                return result;
        }

        /** Horizontal flip the image */
        public Bitmap horizontalFlip(Bitmap bitmap)
        {
            Bitmap result = new Bitmap(bitmap, bitmap.Size);
            int height = bitmap.Height;
            int width = bitmap.Width;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    result.SetPixel(i,height-1-j, Color.FromArgb(bitmap.GetPixel(i, j).R, bitmap.GetPixel(i, j).G, bitmap.GetPixel(i, j).B));
                }
            }
            return result;
        }

        /** Turn the image counterClockwise */
        public Bitmap turnCounterClockWise(Bitmap bitmap)
        {
            int height = bitmap.Height;
            int width = bitmap.Width;

            int new_width = height;
            int new_height = width;
            Bitmap result = new Bitmap(new_width, new_height);

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    result.SetPixel(j, new_height - 1 - i, Color.FromArgb(bitmap.GetPixel(i, j).R, bitmap.GetPixel(i, j).G, bitmap.GetPixel(i, j).B));
                }
            }
            return result;
        }

        /** Turn the image Clockwise */
        public Bitmap turnClockWise(Bitmap bitmap)
        {
            
            int height = bitmap.Height;
            int width = bitmap.Width;

            int new_width = height;
            int new_height = width;
            Bitmap result = new Bitmap(new_width,new_height);

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    result.SetPixel(new_width-1-j, i, Color.FromArgb(bitmap.GetPixel(i, j).R, bitmap.GetPixel(i, j).G, bitmap.GetPixel(i, j).B));
                }
            }
                return result;
        }

        /** Equalize the histogram */
        public Bitmap equalizeHistogram(Bitmap bitmap)
        {
            Bitmap result = new Bitmap(bitmap, bitmap.Size);
            int height = bitmap.Height;
            int width = bitmap.Width;

            /** Calculate the histogram of the image */
            Bitmap grayImage = rgbToGray(bitmap);
            double[] histogram = new double[256];
            double[] cumHistogram = new double[256];
            double N = width * height;

            for (int i = 0; i < grayImage.Width; i++)
            {
                for (int j = 0; j < grayImage.Height; j++)
                {
                    int intensity = grayImage.GetPixel(i, j).R;
                    histogram[intensity] += 1 / N;
                }
            }

            /** Probability density function */
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    cumHistogram[i] += histogram[j];
                }
            }

            

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int red = (int)(cumHistogram[bitmap.GetPixel(i, j).R] * 255);
                    int green = (int)(cumHistogram[bitmap.GetPixel(i, j).G] * 255);
                    int blue = (int)(cumHistogram[bitmap.GetPixel(i, j).B] * 255);
                    result.SetPixel(i, j, Color.FromArgb(red, green, blue));
                }
            }
                return result;

        }

        /** Normalize the histogram */
        public Bitmap normalizeHistogram(Bitmap bitmap)
        {
            Bitmap result = new Bitmap(bitmap, bitmap.Size);
            int height = bitmap.Height;
            int width = bitmap.Width;
            int min = 0;
            int max = 0;

            /** Calculate the histogram of the image */
            Bitmap grayImage = rgbToGray(bitmap);
            double[] histogram = new double[256];


            for (int i = 0; i < grayImage.Width; i++)
            {
                for (int j = 0; j < grayImage.Height; j++)
                {
                    int intensity = grayImage.GetPixel(i, j).R;
                    histogram[intensity] ++;
                    max = intensity > max ? intensity : max;
                    min = intensity < min ? intensity : min;

                }
            }


            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int red = (int)(((bitmap.GetPixel(i, j).R-min) * 255)/(double)(max-min));
                    int green = (int)(((bitmap.GetPixel(i, j).G - min) * 255) / (double)(max - min));
                    int blue = (int)(((bitmap.GetPixel(i, j).B - min) * 255) / (double)(max - min));
                    result.SetPixel(i, j, Color.FromArgb(truncate(red), truncate(green), truncate(blue)));
                }
            }
            return result;

        }


        public Bitmap scaleNearestNeighbour(Bitmap bitmap, double x_scale_factor = 1, double y_scale_factor = 1)
        {
            try
            {
                Bitmap bmp = bitmap;

                int height = bmp.Height;
                int width = bmp.Width;

                int new_height = (int)(height * y_scale_factor);
                int new_width = (int)(width * x_scale_factor);


                //create the resultant bitmap
                Bitmap resultbmp = new Bitmap(new_width, new_height);

                for (int i = 0; i < new_width; i++)
                {
                    for (int j = 0; j < new_height; j++)
                    {
                        resultbmp.SetPixel(i, j, bmp.GetPixel((int)Math.Floor(i / x_scale_factor), (int)Math.Floor(j / y_scale_factor)));
                    }
                }

                return resultbmp;
            }
            catch (Exception Err)
            {
                Console.WriteLine(Err);
                return null;
            }

        }

        public Bitmap biLinear(Bitmap bitmap, double x_scale_factor = 1, double y_scale_factor = 1)
        {
            Bitmap bmp = new Bitmap(bitmap);

            int height = bmp.Height;
            int width = bmp.Width;

            int new_height = (int)(height * y_scale_factor);
            int new_width = (int)(width * x_scale_factor);

            double x_weight;
            double y_weight;

            //create the resultant bitmap
            Bitmap resultbmp = new Bitmap(new_width, new_height);

            for (int i = 0; i < new_width; i++)
            {
                for (int j = 0; j < new_height; j++)
                {
                    x_weight = (i / x_scale_factor) - Math.Floor(i / x_scale_factor);
                    y_weight = (j / y_scale_factor) - Math.Floor(j / y_scale_factor);

                    int floor_x = (int)(Math.Floor(i / x_scale_factor));
                    int ceiling_x = (int)(Math.Ceiling(i / x_scale_factor));

                    int floor_y = (int)(Math.Floor(j / y_scale_factor));
                    int ceiling_y = (int)(Math.Ceiling(j / y_scale_factor));

                    floor_x = floor_x < 0 ? 0 : floor_x;
                    floor_y = floor_y < 0 ? 0 : floor_y;

                    ceiling_x = ceiling_x >= width ? width-1 : ceiling_x;
                    ceiling_y = ceiling_y >= height ? height-1 : ceiling_y;
           
                    Color pixel1 = bmp.GetPixel(floor_x, floor_y);
                    Color pixel2 = bmp.GetPixel(ceiling_x, floor_y);
                    Color pixel3 = bmp.GetPixel(floor_x, ceiling_y);
                    Color pixel4 = bmp.GetPixel(ceiling_x, ceiling_y); 

                    //Blue Color 
                    int b1 = (int)((1 - x_weight) * pixel1.B + x_weight * pixel2.B);
                    int b2 = (int)((1 - x_weight) * pixel3.B + x_weight * pixel4.B);

                    //Green Color 
                    int g1 = (int)((1 - x_weight) * pixel1.G + x_weight * pixel2.G);
                    int g2 = (int)((1 - x_weight) * pixel3.G + x_weight * pixel4.G);

                    //red Color
                    int r1 = (int)((1 - x_weight) * pixel1.R + x_weight * pixel2.R);
                    int r2 = (int)((1 - x_weight) * pixel3.R + x_weight * pixel4.R);

                    int blue = (int)((1 - y_weight) * (double)b1 + y_weight * (double)b2);
                    int red = (int)((1 - y_weight) * (double)r1 + y_weight * (double)r2);
                    int green = (int)((1 - y_weight) * (double)g1 + y_weight * (double)g2);

                    resultbmp.SetPixel(i, j, Color.FromArgb(red, green, blue));

                }
            }
            return resultbmp;
        }

        /** Contrast stretch based alpha, Beta and Gamma*/
        public Bitmap contrastStretch(Bitmap bitmap, double alpha, double beta, double gamma, int a, int b)
        {
            Bitmap bitmapResult = new Bitmap(bitmap, bitmap.Size);
            Bitmap bmp = bitmap;

            int height = bmp.Height;
            int width = bmp.Width;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color color = bitmap.GetPixel(i, j);
                    int grayscaleValue = (int)(0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B);

                    if (grayscaleValue <= a)
                    {
                        //Blue Color
                        int blue = truncate((int)Math.Round(alpha* (bmp.GetPixel(i, j).B )));
                        //Red Color
                        int red = truncate((int)Math.Round(alpha * (bmp.GetPixel(i, j).R )));
                        //Green Color 
                        int green = truncate((int)Math.Round(alpha * (bmp.GetPixel(i, j).G )));

                        bitmapResult.SetPixel(i, j, Color.FromArgb(red, green, blue));
                    }
                    else if (a < grayscaleValue && grayscaleValue <= b)
                    {
                        //Blue Color
                        int blue = truncate((int)Math.Round(beta * (bmp.GetPixel(i, j).B - a) + alpha * a));
                        //Red Color
                        int red = truncate((int)Math.Round(beta * (bmp.GetPixel(i, j).R - a) + alpha * a));
                        //Green Color 
                        int green = truncate((int)Math.Round(beta * (bmp.GetPixel(i, j).G - a) + alpha * a));

                        bitmapResult.SetPixel(i, j, Color.FromArgb(red, green, blue));
                    }
                    else
                    {
                        //Blue Color
                        int blue = truncate((int)Math.Round(gamma * (bmp.GetPixel(i, j).B - b) + beta * (b - a) + alpha * a));
                        //Red Color
                        int red = truncate((int)Math.Round(gamma * (bmp.GetPixel(i, j).R- b) +beta * (b - a) + alpha * a));
                        //Green Color 
                        int green = truncate((int)Math.Round(gamma* (bmp.GetPixel(i, j).G - b) +beta * (b - a) + alpha * a));

                        bitmapResult.SetPixel(i, j, Color.FromArgb(red, green, blue));

                    }
                }
            }

            return bitmapResult;
        }

        public Bitmap bitSlice(Bitmap bitmap, int plane)
        {
            Bitmap result = new Bitmap(bitmap);
            Bitmap bmp = new Bitmap(bitmap);

            bmp = rgbToGray(bitmap);

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    int intensity = bmp.GetPixel(i, j).R;
                    String binary = Convert.ToString(intensity, 2);
                    int value;
                    if (binary.Length-1 < plane)
                    {
                        value = 0;
                    }
                    else
                    {
                        value= binary[binary.Length-1 - plane];
                    }
                    
                    result.SetPixel(i, j, Color.FromArgb(value,value,value));
                }
            }
            return result;
        }
        private Bitmap zeroPad(Bitmap bitmap, int n)
        {
            Bitmap bmp = new Bitmap(bitmap);
            Bitmap result = new Bitmap(bmp.Width + (n-1), bmp.Height + (n-1));

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    result.SetPixel(i + n/2, j + n/2, bmp.GetPixel(i, j));
                }
            }
            return result;
        }

        public Bitmap meanFilter(Bitmap bitmap, int mask)
        {
            Bitmap bmp = new Bitmap(zeroPad(bitmap,mask));
            Bitmap result = new Bitmap(bitmap);
            int increment = mask / 2;
            int red = 0;
            int green = 0;
            int blue = 0;

            double weight = mask * mask;

            for (int i = increment; i < (bmp.Width-1 - increment); i++)
            {
                for (int j = increment; j <(bmp.Height-1 - increment); j++)
                {
                    red = 0; green = 0; blue = 0;
                    for (int m = i - increment; m <= i + increment; m++)
                    {
                        for (int n = j - increment; n <= j + increment; n++)
                        {
                            red += bmp.GetPixel(m,n).R;
                            green+=bmp.GetPixel(m,n).G;
                            blue += bmp.GetPixel(m, n).B;
                        }
                    }
                    
                    result.SetPixel(i-increment, j-increment, Color.FromArgb((int)(red / weight), (int)(green / weight), (int)(blue / weight)));
                }
               
            }
            return result;

        }

        public Bitmap logFilter(Bitmap bitmap, int mask)
        {
            Bitmap bmp = new Bitmap(zeroPad(bitmap, mask));
            Bitmap result = new Bitmap(bitmap);
            int increment = mask / 2;
            int red = 0;
            int green = 0;
            int blue = 0;
           

            for (int i = increment; i < (bmp.Width - 1 - increment); i++)
            {
                for (int j = increment; j < (bmp.Height - 1 - increment); j++)
                {
                    red = 0; green = 0; blue = 0;
                    for (int m = i - increment; m <= i + increment; m++)
                    {
                        for (int n = j - increment; n <= j + increment; n++)
                        {
                            if (m == i && n == j)
                            {
                                red -= 4*bmp.GetPixel(m, n).R;
                                green -= 4*bmp.GetPixel(m, n).G;
                                blue -= 4*bmp.GetPixel(m, n).B;
                            }
                            else if ((m == i && n == j - 1) || m == i + 1 && n == j || m == i && n == j + 1 || m == i - 1 && n == j)
                            {
                                red += bmp.GetPixel(m, n).R;
                                green += bmp.GetPixel(m, n).G;
                                blue += bmp.GetPixel(m, n).B;
                            }
                            
                        }
                    }
                    int grayscaleValue = (int)(0.2126 * red + 0.7152 * green + 0.0722 * blue);
                    if (grayscaleValue > 2)
                    {
                        result.SetPixel(i - increment, j - increment, Color.FromArgb(255, 255, 255));
                    }
                    else
                    {
                        result.SetPixel(i - increment, j - increment, Color.FromArgb(0, 0, 0));
                    }
                   
                }

            }
            return result;

        }

        public Bitmap userFilter(Bitmap bitmap, int number, double[,] mask)
        {
            Bitmap bmp = new Bitmap(zeroPad(bitmap, number));
            Bitmap result = new Bitmap(bitmap);
            int increment = number / 2;
            double red = 0;
            double green = 0;
            double blue = 0;

            double weight=0;

            for (int i = 0; i < number; i++)
            {
                for (int j = 0; j < number; j++)
                {
                    weight += mask[i, j];
                }
            }

                for (int i = increment; i < (bmp.Width - 1 - increment); i++)
                {
                    for (int j = increment; j < (bmp.Height - 1 - increment); j++)
                    {
                        red = 0; green = 0; blue = 0;
                        for (int m = i - increment; m <= i + increment; m++)
                        {
                            for (int n = j - increment; n <= j + increment; n++)
                            {
                                red += bmp.GetPixel(m, n).R * mask[m + increment - i, n + increment - j];
                                green += bmp.GetPixel(m, n).G * mask[m + increment - i, n + increment - j];
                                blue += bmp.GetPixel(m, n).B * mask[m + increment - i, n + increment - j];
                            }
                        }

                        result.SetPixel(i - increment, j - increment, Color.FromArgb((int)(red / weight), (int)(green / weight), (int)(blue / weight)));
                    }

                }
            return result;

        }

        public void HuffmanEncode(Bitmap bitmap)
        {
            Bitmap bmp = new Bitmap(bitmap);
            Bitmap grayImage = rgbToGray(bmp);
            int[] histogram = new int[256];
            hashTable = new Hashtable();
            int count = 0;

            for (int i = 0; i < grayImage.Width; i++)
            {
                for (int j = 0; j < grayImage.Height; j++)
                {
                    int intensity = grayImage.GetPixel(i, j).R;
                    histogram[intensity]++;
                }
            }

            // count the intensities with a frequency 
            for(int i=0;i<256;i++){
                if(histogram[i]!=0){
                    count++;
                }
            }

            int[] huffmanHistogram = new int[count];
            int index = 0;

            //move the values to a new array
            for (int i = 0; i < 256; i++)
            {
                if (histogram[i] != 0)
                {
                    huffmanHistogram[index] = histogram[i];
                    hashTable.Add(index+1, i);
                    index++;
                }
            }

            doHuffmanEncoding(huffmanHistogram,count);
            return;
        }


        private void treeTraverse(Tree tree, string code)
        {
            if (tree.Value != 0)
            {
                string print = hashTable[tree.Value] + " Code is: " + code;
                //Console.WriteLine(print);                       // Prints the code to the console
                System.IO.StreamWriter file = new System.IO.StreamWriter("Encoded.txt",true);
                file.WriteLine(print);
                file.Close();
                return;
            }

            treeTraverse(tree.treeLeft, code + "0");
            treeTraverse(tree.treeRight, code + "1");
        }

        private void doHuffmanEncoding(int[] array, int count)
        {
            LinkedList<Node> list = new LinkedList<Node>();
            IEnumerable<Node> orderedList = new LinkedList<Node>();
            int[] frequency = array;
            int n = count;

            for (int i = 0; i < n; i++)
            {
                Tree tree = new Tree(i + 1);
                Node node = new Node(tree, frequency[i]);
                list.AddFirst(node);
            }

            for (int j = 0; j < n - 1; j++)
            {
                LinkedList<Node> newList = new LinkedList<Node>();
                orderedList = list.OrderByDescending(p => p.frequency);
                foreach (var item in orderedList)
                {
                    newList.AddFirst(item);
                }

                Tree newTree = new Tree(newList.ElementAt(0).tree, newList.ElementAt(1).tree);
                Node newNode = new Node(newTree, (newList.ElementAt(0).frequency + newList.ElementAt(1).frequency));
                newList.RemoveFirst();
                newList.RemoveFirst();
                newList.AddFirst(newNode);
                list = newList;
            }

            //Delete Existing data in the file
            System.IO.StreamWriter file = new System.IO.StreamWriter("Encoded.txt");
            file.WriteLine("----- Huffman Encoded Values ----");
            file.Close();
         
            string code = null;
            Tree traverse = list.ElementAt(0).tree;
            treeTraverse(traverse, code);
        }

    }

   
}
