from __future__ import absolute_import
from __future__ import division
from __future__ import print_function

import numpy as np
import tensorflow as tf
import ConvNet
import sys 
import io
inputdata = sys.argv[1]
tf.logging.set_verbosity(tf.logging.INFO)

def cnn_model_fn(features, labels, mode):
  """Model function for CNN."""
  # Input Layer
  # Reshape X to 4-D tensor: [batch_size, width, height, channels]
  # MNIST images are 28x28 pixels, and have one color channel
  input_layer = tf.reshape(features["x"], [-1, 16, 16, 16, 1])

  # Convolutional Layer #1
  # Computes 32 features using a 5x5 filter with ReLU activation.
  # Padding is added to preserve width and height.
  # Input Tensor Shape: [batch_size, 28, 28, 1]
  # Output Tensor Shape: [batch_size, 28, 28, 32]
  conv1 = tf.layers.conv3d(
      inputs=input_layer,
      filters=32,
      kernel_size=[5, 5, 5],
      padding="same",
      activation=tf.nn.relu)

  # Pooling Layer #1
  # First max pooling layer with a 2x2 filter and stride of 2
  # Input Tensor Shape: [batch_size, 28, 28, 32]
  # Output Tensor Shape: [batch_size, 14, 14, 32]
  pool1 = tf.layers.max_pooling3d(inputs=conv1, pool_size=[2, 2, 2], strides=2)

  # Convolutional Layer #2
  # Computes 64 features using a 5x5 filter.
  # Padding is added to preserve width and height.
  # Input Tensor Shape: [batch_size, 14, 14, 32]
  # Output Tensor Shape: [batch_size, 14, 14, 64]
  conv2 = tf.layers.conv3d(
      inputs=pool1,
      filters=64,
      kernel_size=[5, 5, 5],
      padding="same",
      activation=tf.nn.relu)

  # Pooling Layer #2
  # Second max pooling layer with a 2x2 filter and stride of 2
  # Input Tensor Shape: [batch_size, 14, 14, 64]
  # Output Tensor Shape: [batch_size, 7, 7, 64]
  pool2 = tf.layers.max_pooling3d(inputs=conv2, pool_size=[2, 2, 2], strides=2)

    # Convolutional Layer #3
  # Computes 64 features using a 5x5 filter.
  # Padding is added to preserve width and height.
  # Input Tensor Shape: [batch_size, 14, 14, 32]
  # Output Tensor Shape: [batch_size, 14, 14, 64]
  conv3 = tf.layers.conv3d(
      inputs=pool2,
      filters=128,
      kernel_size=[5, 5, 5],
      padding="same",
      activation=tf.nn.relu)

  # Pooling Layer #3
  pool3 = tf.layers.max_pooling3d(inputs=conv3, pool_size=[2, 2, 2], strides=2)

  
  # Flatten tensor into a batch of vectors
  # Input Tensor Shape: [batch_size, 7, 7, 64]
  # Output Tensor Shape: [batch_size, 7 * 7 * 64]
  pool3_flat = tf.reshape(pool3, [-1, 2*2*2*128])


  # Dense Layer
  # Densely connected layer with 1024 neurons
  # Input Tensor Shape: [batch_size, 7 * 7 * 64]
  # Output Tensor Shape: [batch_size, 1024]
  dense = tf.layers.dense(inputs=pool3_flat, units=1024, activation=tf.nn.relu)

  # Add dropout operation; 0.6 probability that element will be kept
  dropout = tf.layers.dropout(
      inputs=dense, rate=0.4, training=mode == tf.estimator.ModeKeys.TRAIN)

  # Logits layer
  # Input Tensor Shape: [batch_size, 1024]
  # Output Tensor Shape: [batch_size, 10]
  #units used to be =14
  logits = tf.layers.dense(inputs=dropout, units=102)
  maxlayer = tf.nn.softmax(logits)
  I = tf.linspace(1.0,102.0, 102)
  I = tf.expand_dims(I,1)
  logits1 = tf.matmul(maxlayer,I)
  logits2 = tf.layers.dense(inputs=dropout, units=1)

  predictions = {
      # Generate predictions (for PREDICT and EVAL mode)
      "classes1": logits1,
      "classes2": logits2,
      # Add `softmax_tensor` to the graph. It is used for PREDICT and by the
      # `logging_hook`.
      "probabilities": tf.nn.softmax(logits, name="softmax_tensor")
  }
  if mode == tf.estimator.ModeKeys.PREDICT:
    return tf.estimator.EstimatorSpec(mode=mode, predictions=predictions)

  # Calculate Loss (for both TRAIN and EVAL modes)
  onehot_labels = tf.one_hot(indices=tf.cast(labels, tf.int32), depth=16)
  loss = tf.losses.softmax_cross_entropy(
      onehot_labels=onehot_labels, logits=logits)

  # Configure the Training Op (for TRAIN mode)
  if mode == tf.estimator.ModeKeys.TRAIN:
    optimizer = tf.train.GradientDescentOptimizer(learning_rate=0.001)
    train_op = optimizer.minimize(
        loss=loss,
        global_step=tf.train.get_global_step())
    return tf.estimator.EstimatorSpec(mode=mode, loss=loss, train_op=train_op)

  # Add evaluation metrics (for EVAL mode)
  eval_metric_ops = {
      "accuracy": tf.metrics.accuracy(
          labels=labels, predictions=predictions["classes"])}
  return tf.estimator.EstimatorSpec(
      mode=mode, loss=loss, eval_metric_ops=eval_metric_ops)

def main(unused_argv):
  # Load training and eval data
 # mnist = tf.contrib.learn.datasets.load_dataset("mnist")
    predict_data = ConvNet.reformat(inputdata)
    
    # Create the Estimator
    DL_classifier = tf.estimator.Estimator(
        model_fn=cnn_model_fn, model_dir=ConvNet.networkLocation)

    # Set up logging for predictions
    # Log the values in the "Softmax" tensor with label "probabilities"
    # tensors_to_log = {"probabilities": "softmax_tensor"}
    # logging_hook = tf.train.LoggingTensorHook(
    #   tensors=tensors_to_log, every_n_iter=50)

    # Print out predictions
    predict_input_fn = tf.estimator.inputs.numpy_input_fn(
        x={"x": predict_data},
        num_epochs=1,
        shuffle=False)
    predict_results = DL_classifier.predict(input_fn=predict_input_fn)
    for i, p in enumerate(predict_results):
        print(round(p["classes1"][0]*0.5+p["classes2"][0]*0.5))
        #print("Prediction %s: %s" % (i + 1, p["classes"]))



if __name__ == "__main__":
    tf.app.run()

