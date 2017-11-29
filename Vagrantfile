Vagrant.configure("2") do |config|
  config.vm.box = "xe"
  #we use our local box
  #config.vm.box_url = "../../xe.box"
  config.vm.box_url = "deps/kikitux/packer-oraclelinux-ovf/xe.box"
  config.vm.network "forwarded_port", guest: 1521, host: 1521
  config.vm.network "private_network", ip: "192.168.56.11"
  #config.vm.hostname = "xe"
  config.vm.provider "virtualbox"
  config.vm.provision "shell", path: "provision/script.sh"
end
