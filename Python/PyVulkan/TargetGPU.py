import vulkan as vk

# Instance Vulkan
app_info = vk.VkApplicationInfo(
    pApplicationName="Multi-GPU App",
    applicationVersion=1,
    pEngineName="No Engine",
    engineVersion=1,
    apiVersion=vk.VK_API_VERSION_1_0,
)
instance = vk.vkCreateInstance(vk.VkInstanceCreateInfo(pApplicationInfo=app_info))

# Liste des GPU disponibles
gpus = vk.vkEnumeratePhysicalDevices(instance)
for i, gpu in enumerate(gpus):
    print(f"GPU {i}: {vk.vkGetPhysicalDeviceProperties(gpu).deviceName}")

# Sélectionnez un GPU (par exemple, GPU 0)
selected_gpu = gpus[0]
