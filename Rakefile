desc 'Start web server'
task 'start-server' do
	cmd = "tools\\IISExpress\\iisexpress.exe /path:#{File.dirname(__FILE__).gsub("/","\\")}\\src\\site /port:3000"
	exec cmd
end