#!/bin/sh

for i in `ls *Card*.jpg`; do
    convert $i -resample 300x300 $i
    convert $i -resize 1050x1500 $i
done
