
./protoc  --proto_path ../res/proto --proto_path ./ --cpp_out ../proto/ tsv2pb.proto tools.proto

if [ $? -ne 0 ]; then
    echo "generate_pb error!"
    exit -1
fi