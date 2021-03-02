local sprite = app.activeSprite

-- Check constraints
if sprite == nil then
  app.alert("No Sprite...")
  return
end
if sprite.colorMode ~= ColorMode.INDEXED then
  app.alert("Sprite needs to be indexed")
  return
end

local function getIndexData(img, x, y, w, h)
	local res = ""
	for y = 0,h-1 do
		for x = 0, w-1 do
			px = img:getPixel(x, y)
			res = res .. string.format("%i ", px)
		end
		if y ~= h-1 then
			res = res .. "\n"
		end
	end

	return res
end

local function exportFrame(frm)
	if frm == nil then
		frm = 1
	end

	local img = Image(sprite.spec)
	img:drawSprite(sprite, frm)
	
	io.write(getIndexData(img, x, y, sprite.width, sprite.height))
end


local dlg = Dialog()
dlg:file{ id="exportFile",
          label="File",
          title="SPR File Export",
          open=false,
          save=true,
        --filename= p .. fn .. "pip",
          filetypes={"spr" }}
dlg:check{ id="onlyCurrentFrame",
           text="Export only current frame",
           selected=false }

dlg:button{ id="ok", text="OK" }
dlg:button{ id="cancel", text="Cancel" }
dlg:show()
local data = dlg.data
if data.ok then
	if data.onlyCurrentFrame then
		local f = io.open(data.exportFile, "w")
		io.output(f)
		exportFrame(app.activeFrame)
		io.close(f)
	else
		for i = 1,#sprite.frames do
			local s = string.gsub(data.exportFile, "%.", (("_" .. i) .. "."))
			local f = io.open(s, "w")
			io.output(f)
			exportFrame(i)
			io.close(f)
		end
	end
end