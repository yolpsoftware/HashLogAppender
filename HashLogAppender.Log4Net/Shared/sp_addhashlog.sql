create procedure AddHashLog
    (@smallhash varchar(64), @bighash varchar(64), @logger varchar(max), @date datetime2(7), @level tinyint, @message varchar(max)) as

begin tran
   update HashLog with (serializable) set [date] = @date, [count] = [count] + 1
   where bighash = @bighash

   if @@rowcount = 0
   begin
       insert HashLog (smallhash, bighash, logger, [date], [level], message, [count]) values (@smallhash, @bighash, @logger, @date, @level, @message, 1)
   end
commit tran

