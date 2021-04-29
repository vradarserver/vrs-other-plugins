#!/bin/bash
set -e
VER=v2
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
WORKROOT=$DIR/work-plugin-airportdata-thumbnails-v2
WORK=$WORKROOT/Plugins/AirportDataThumbnails
ROOT=$DIR/../..
BUILD=$ROOT/Plugin.AirportDataThumbnails-v2/bin/Release
OUTPUT=$DIR/output

if [ -d $WORKROOT ]; then
  rm -r $WORKROOT;
fi
if [ ! -d $OUTPUT ]; then
  mkdir $OUTPUT;
fi
mkdir -p $WORK

cp $ROOT/LICENSE.txt $WORK
cp $BUILD/README.md $WORK
cp $BUILD/VirtualRadar.Plugin.AirportDataThumbnails.dll $WORK
cp $BUILD/VirtualRadar.Plugin.AirportDataThumbnails.xml $WORK
cd $WORKROOT
tar -czf     $OUTPUT/Plugin-AirportDataThumbnails-$VER.tar.gz *
echo Created $OUTPUT/Plugin-AirportDataThumbnails-$VER.tar.gz
