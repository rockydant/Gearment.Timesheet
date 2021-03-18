ALTER TABLE [GearmentTimesheetData]
DROP CONSTRAINT [DF__GearmentT__Total__540C7B00]

ALTER TABLE [GearmentTimesheetData]
ALTER COLUMN TotalWorkingHour DECIMAL(20,1)

ALTER TABLE [GearmentTimesheetData]
DROP CONSTRAINT [DF__GearmentT__Total__4C6B5938]

ALTER TABLE [GearmentTimesheetData]
ALTER COLUMN TotalRestHour DECIMAL(20,1)

ALTER TABLE [GearmentEmployee]
DROP CONSTRAINT [DF__GearmentEm__Rate__123EB7A3]

ALTER TABLE [GearmentEmployee]
ALTER COLUMN Rate DECIMAL(20,1)

ALTER TABLE [GearmentTimesheetData]
DROP CONSTRAINT [DF__GearmentTi__Rate__45BE5BA9]

ALTER TABLE [GearmentTimesheetData]
ALTER COLUMN Rate DECIMAL(20,1)