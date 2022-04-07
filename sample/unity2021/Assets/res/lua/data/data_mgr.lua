---@class data_mgr
data_mgr = {}

require 'Game.Data.data_mgr.data_mgr_require'

---@public
function data_mgr:init()
    self.pb = require 'pb'
    --加载二进制pb数据
    local data = kss.ProtoReader.GetInstance():LoadPbFile("RawData/Proto/proto_res.bytes")
    self.pb.load(data)
    self.pb.option('enum_as_value')
    
    self:load_all()
end

---@public
function data_mgr:release()
    self.pb = nil
end

---@private
function data_mgr:load_pb(name, array, key1, key2)
    local data = kss.ProtoReader.GetInstance():LoadPbFile('RawData/excel/'..name..'.bytes')
    local cmd = self.pb.decode(array, data)
    local tmp = {}
    if key2 == nil then
        --单主键
        for _, v in ipairs(cmd.items) do
            tmp[v[key1]] = v
        end
    else
        --双主键
        for _, v in ipairs(cmd.items) do
            if tmp[v[key1]] == nil then
                tmp[v[key1]] = {}
            end
            tmp[v[key1]][v[key2]] = v
        end
    end
    return tmp
end
