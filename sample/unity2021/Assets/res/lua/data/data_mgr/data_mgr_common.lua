-- THIS SOURCE CODE WAS AUTO-GENERATED BY TOOL, DO NOT MODIFY IT!!!

---@public 
---@return RES.ResRes[]
function data_mgr:load_res_all()
    if not self.res_db then
        self.res_db = self:load_pb('res', 'RES.ResResArray', 'id_key')
    end
    return self.res_db
end
---@public 
---@return RES.ResRes
function data_mgr:load_res(key)
    if not self.res_db then
        self:load_res_all()
    end
    return self.res_db[key]
end

