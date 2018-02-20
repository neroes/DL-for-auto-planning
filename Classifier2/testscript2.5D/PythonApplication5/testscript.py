""" 2.5D Convolutional Neural Network Estimator for Pokuban levels length heuristic, built with tf.layers."""
# Based on tf.layers example found on https://www.tensorflow.org/tutorials/layers

from __future__ import absolute_import
from __future__ import division
from __future__ import print_function

import numpy as np
import tensorflow as tf
import ConvNet
import sys

tf.logging.set_verbosity(tf.logging.INFO)


def cnn_model_fn(features, labels, mode):
  """Model function for CNN."""
  # Input Layer
  # Reshape X to 4-D tensor: [batch_size, width, height, channels]
  # Pukoban levels are 16x16 pixels, and have 16 channels
  input_layer = tf.reshape(features["x"], [-1, 16, 16, 16])

  # Convolutional Layer #1
  # Computes 32 features using a 5x5 filter with ReLU activation.
  # Padding is added to preserve width and height.
  # Input Tensor Shape: [batch_size, 16, 16, 16]
  # Output Tensor Shape: [batch_size, 16, 16, 32]
  conv1 = tf.layers.conv2d(
      inputs=input_layer,
      filters=32,
      kernel_size=[5, 5],
      padding="same",
      activation=tf.nn.relu)

  # Pooling Layer #1
  # First max pooling layer with a 2x2 filter and stride of 2
  # Input Tensor Shape: [batch_size, 16, 16, 32]
  # Output Tensor Shape: [batch_size, 8, 8, 32]
  pool1 = tf.layers.max_pooling2d(inputs=conv1, pool_size=[2, 2], strides=2)
  

  # Reshaping the tensor to increase the dimension by 1 so it fits the conv3d input size.
  layer3d = tf.reshape(pool1,[-1,8,8,8,4])

  # Convolutional Layer #2
  # Computes 64 features using a 5x5x5 filter.
  # Padding is added to preserve width and height.
  # Input Tensor Shape: [batch_size, 8, 8, 8, 32]
  # Output Tensor Shape: [batch_size, 8, 8, 8, 64]
  conv2 = tf.layers.conv3d(
      inputs=layer3d,
      filters=64,
      kernel_size=[5, 5, 5],
      padding="same",
      activation=tf.nn.relu)

  # Pooling Layer #2
  # Second max pooling layer with a 2x2x2 filter and stride of 2
  # Input Tensor Shape: [batch_size, 8, 8, 8, 64]
  # Output Tensor Shape: [batch_size, 4, 4, 4, 64]
  pool2 = tf.layers.max_pooling3d(inputs=conv2, pool_size=[2, 2, 2], strides=2)

  # Convolutional Layer #3
  # Computes 128 features using a 5x5x5 filter.
  # Padding is added to preserve width and height.
  # Input Tensor Shape: [batch_size, 4, 4, 4, 64]
  # Output Tensor Shape: [batch_size, 4, 4, 4, 128]
  conv3 = tf.layers.conv3d(
      inputs=pool2,
      filters=128,
      kernel_size=[5, 5, 5],
      padding="same",
      activation=tf.nn.relu)

  # Pooling Layer #3
  # Third max pooling layer with a 2x2x2 filter and stride of 2
  # Input Tensor Shape: [batch_size, 4, 4, 4, 128]
  # Output Tensor Shape: [batch_size, 2, 2, 2, 128]
  pool3 = tf.layers.max_pooling3d(inputs=conv3, pool_size=[2, 2, 2], strides=2)

  
  # Flatten tensor into a batch of vectors
  # Input Tensor Shape: [batch_size, 2, 2, 64]
  # Output Tensor Shape: [batch_size, 2 * 2 * 2 * 128]
  pool3_flat = tf.reshape(pool3, [-1, 2*2*2*128])


  # Dense Layer
  # Densely connected layer with 1024 neurons
  # Input Tensor Shape: [batch_size, 2 * 2 * 2 * 128]
  # Output Tensor Shape: [batch_size, 1024]
  dense = tf.layers.dense(inputs=pool3_flat, units=1024, activation=tf.nn.relu)

  # Add dropout operation; 0.6 probability that element will be kept
  dropout = tf.layers.dropout(
      inputs=dense, rate=0.4, training=mode == tf.estimator.ModeKeys.TRAIN)

  # Overall Logits layer for "Classification model"
  # Input Tensor Shape: [batch_size, 1024]
  # Output Tensor Shape: [batch_size, 102]
  logits = tf.layers.dense(inputs=dropout, units=102)
  maxlayer = tf.nn.softmax(logits)
  I = tf.linspace(1.0,102.0, 102)
  I = tf.expand_dims(I,1)

  # "Classification model" single value output
  logits1 = tf.matmul(maxlayer,I)

  # Logits for "Regression model"
  logits2 = tf.layers.dense(inputs=dropout, units=1)


  predictions = {
      # Generate predictions (for PREDICT and EVAL mode)
      "classes1": logits1, #Classification model
      "classes2": logits2, #Regression model
      # Add `softmax_tensor` to the graph. It is used for PREDICT and by the
      # `logging_hook`.
      "probabilities": tf.nn.softmax(logits, name="softmax_tensor")
  }
  
  
  if mode == tf.estimator.ModeKeys.PREDICT:
    return tf.estimator.EstimatorSpec(mode=mode, predictions=predictions)

  # Calculate Loss (for both TRAIN and EVAL modes)
  onehot_labels = tf.one_hot(indices=tf.cast(labels, tf.int32), depth=102)
  # Calculate loss for "Classification model"
  loss1 = tf.log(tf.losses.softmax_cross_entropy(
      onehot_labels=onehot_labels, logits=logits))+0.0000001
  # Calculate loss for "Regression model"
  loss2 = tf.losses.mean_squared_error(labels, tf.reshape(logits2,[-1]))

  # Calculate overall loss
  loss = loss1*0.5+loss2*0.5


  global_step = tf.Variable(0, trainable=False)
  starter_learning_rate = 0.0005
  learning_rate = tf.train.exponential_decay(starter_learning_rate, global_step,
                                           100000, 0.90, staircase=True)
  # Configure the Training Op (for TRAIN mode)
  if mode == tf.estimator.ModeKeys.TRAIN:
    optimizer = tf.train.GradientDescentOptimizer(learning_rate)
    train_op = optimizer.minimize(
        loss=loss,
        global_step=tf.train.get_global_step())
    return tf.estimator.EstimatorSpec(mode=mode, loss=loss, train_op=train_op)

  # Add evaluation metrics (for EVAL mode)
  eval_metric_ops = {
      "accuracy1": tf.metrics.accuracy(
          labels=labels, predictions=predictions["classes1"]),
      "accuracy2": tf.metrics.accuracy(
          labels=labels, predictions=predictions["classes2"])}
  return tf.estimator.EstimatorSpec(
      mode=mode, loss=loss, eval_metric_ops=eval_metric_ops)


def main(unused_args):
  # Load training and eval data
  train_data = ConvNet.xtrain 
  train_labels = ConvNet.ytrain
  eval_data = ConvNet.xeval
  eval_labels = ConvNet.yeval
  eval_small = ConvNet.xsmalleval
  eval_small_labels = ConvNet.ysmalleval
  eval_large = ConvNet.xlargeeval
  eval_large_labels = ConvNet.ylargeeval
  # Create the Estimator
  DL_classifier = tf.estimator.Estimator(
      model_fn=cnn_model_fn, model_dir="/tmp/Classifier2/2.5D")

  # Set up logging for predictions
  # Log the values in the "Softmax" tensor with label "probabilities"
  tensors_to_log = {"probabilities": "softmax_tensor"}
  logging_hook = tf.train.LoggingTensorHook(
      tensors=tensors_to_log, every_n_iter=50)

  # Train the model
  train_input_fn = tf.estimator.inputs.numpy_input_fn(
      x={"x": train_data},
      y=train_labels,
      batch_size=100,
      num_epochs=None,
      shuffle=True)
  DL_classifier.train(
      input_fn=train_input_fn,
      steps=numOfSteps,
      hooks=[logging_hook])

  # Evaluate the model and print results for "dissimiliar" levels
  eval_dissim = tf.estimator.inputs.numpy_input_fn(
      x={"x": eval_data},
      y=eval_labels,
      num_epochs=1,
      shuffle=False)
  eval_results = DL_classifier.evaluate(input_fn=eval_dissim)
  print(eval_results)

  # Evaluate the model and print results for "small" levels
  eval_smalllvl = tf.estimator.inputs.numpy_input_fn(
      x={"x": eval_small},
      y=eval_small_labels,
      num_epochs=1,
      shuffle=False)
  eval_small_results = DL_classifier.evaluate(input_fn=eval_smalllvl)
  print(eval_small_results)

  # Evaluate the model and print results for "large" levels
  eval_largelvl = tf.estimator.inputs.numpy_input_fn(
      x={"x": eval_large},
      y=eval_large_labels,
      num_epochs=1,
      shuffle=False)
  eval_large_results = DL_classifier.evaluate(input_fn=eval_largelvl)
  print(eval_large_results)

  # Functions to calculate prediction results to easier get readable output
 
  # prediction for dissimilar levels
  predict_input_fn = tf.estimator.inputs.numpy_input_fn(
        x={"x": eval_data},
        num_epochs=1,
        shuffle=False)
  predict_results = DL_classifier.predict(input_fn=predict_input_fn)
  
  # Prediction for small levels
  predict_small_input_fn = tf.estimator.inputs.numpy_input_fn(
        x={"x": eval_small},
        num_epochs=1,
        shuffle=False)
  predict_small_results = DL_classifier.predict(input_fn=predict_small_input_fn)

  # Prediction for large levels
  predict_large_input_fn = tf.estimator.inputs.numpy_input_fn(
        x={"x": eval_large},
        num_epochs=1,
        shuffle=False)
  predict_large_results = DL_classifier.predict(input_fn=predict_large_input_fn)

  # Writing the predictions to a txt file

   # Results file for "dissimilar" levels
  l = 0
  f = open("results.txt",'w')
  for i, p in enumerate(predict_results):
    if (eval_labels[l]==p['classes1']):
      f.write("1")
    else:
      f.write("0")
    f.write(" ")
    if (eval_labels[l]==p['classes2']):
      f.write("1")
    else:
      f.write("0")
    f.write(" "+str(p['classes1'])+ " "+str(p['classes2'])+ " "+ str(eval_labels[l])+ " " + str(ConvNet.evalName[l][:]) + "\n")
    l=l+1
  f.close()

  # Results file for "small" levels
  l = 0
  f = open("smallresults.txt",'w')
  for i, p in enumerate(predict_small_results):
    if (eval_labels[l]==p['classes1']):
      f.write("1")
    else:
      f.write("0")
    f.write(" ")
    if (eval_labels[l]==p['classes2']):
      f.write("1")
    else:
      f.write("0")
    f.write(" "+str(p['classes1'])+ " "+str(p['classes2'])+ " "+ str(eval_labels[l])+ " " + str(ConvNet.evalName[l][:]) + "\n")
    l=l+1
  f.close()


  # Results file for "large" levels
  l = 0
  f = open("largeresults.txt",'w')
  for i, p in enumerate(predict_large_results):
    if (eval_labels[l]==p['classes1']):
      f.write("1")
    else:
      f.write("0")
    f.write(" ")
    if (eval_labels[l]==p['classes2']):
      f.write("1")
    else:
      f.write("0")
    f.write(" "+str(p['classes1'])+ " "+str(p['classes2'])+ " "+ str(eval_labels[l])+ " " + str(ConvNet.evalName[l][:]) + "\n")
    l=l+1
  f.close()

if __name__ == "__main__":
  if len(sys.argv)==1:
        numOfSteps=20000
  else:
        numOfSteps=int(sys.argv[1])
  tf.app.run()
