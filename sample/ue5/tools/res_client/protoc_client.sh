#!/bin/bash

cd `dirname $0`
./protoc --xlsx_input=./excel --tsv_path=./tsv/client --proto_path=./proto/client --store_path=./store/client --store_suffix=bytes
if [ $? -ne 0 ]; then
    echo "xls2pb_cpp error!"
    exit -1
fi