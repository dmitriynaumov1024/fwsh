<template>
<div class="card margin-bottom-1" style="width: 322px">
    <div :style="{ backgroundColor: color.rgbCode, height: '180px' }"></div>
    <div class="pad-05">
        <p><b>{{color.name}}</b></p>
        <p>{{color.rgbCode}}</p>
    </div>
</div>
</template>

<script>
const props = {
    color: {
        type: Object,
        required: true
    }
}

export default {
    props
}
</script>