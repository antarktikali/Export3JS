# Change this to your imagemagick dir
IMAGEMAGICK_DIR=/c/imagemagick
CONVERT_BIN=$IMAGEMAGICK_DIR/convert.exe

# Path where the textures are located. We assume that the path has only texutre
# files.
INPUT_DIR=./Assets
# Folder where the original image files will be copied to
BACKUP_DIR=./original-textures

# Resize the resolution to fit 512x512 if the image is larger than that
#CONVERT_OPTIONS="-resize 512x512>"
# Resize the image to 256x256
CONVERT_OPTIONS="-resize 256x256!"

mkdir $BACKUP_DIR
cp -r $INPUT_DIR $BACKUP_DIR/$INPUT_DIR
echo $CONVERT_BIN
# Get all the files(not folders) in the input dir. We assume that there are only
# image files that are compatible with imagemagick in that path.

TOTAL_FILE_COUNT=`find $INPUT_DIR -type f | wc -l`
counter=1
for f in `find $INPUT_DIR -type f`
do
	echo "Processing file $counter of $TOTAL_FILE_COUNT"
	$CONVERT_BIN $f $CONVERT_OPTIONS $f.png
	rm $f
	let "counter++"
done

echo "Done!"

